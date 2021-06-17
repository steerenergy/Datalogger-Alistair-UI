import sqlite3
import logObjects as lgOb

# Global database variable holds the path to database
database = r"logs.db"


# Creates the main table in the database if it doesn't already exist
def setupDatabase():
    global database
    # SQL statement to create main table
    # This table will hold all the log meta data
    sql_create_main_table = """CREATE TABLE IF NOT EXISTS main (
                                        id integer PRIMARY KEY NOT NULL,
                                        name text NOT NULL,
                                        date text NOT NULL,
                                        time real NOT NULL,
                                        logged_by text,
                                        downloaded_by text);"""
    # Connect to database and execute SQL statement
    conn = sqlite3.connect(database)
    conn.cursor().execute(sql_create_main_table)
    conn.commit()
    conn.close()


# Write new log data to the database
# (Objective 12.2)
def WriteLogData(logComp,headerList):
    global database
    # Create new data table using log id
    # Column names are generated dynamically using headerList
    sqlStatement = 'CREATE TABLE ' + 'data' + str(logComp.id) + '('
    for header in headerList:
        sqlStatement += header + ' REAL,'
    sqlStatement = sqlStatement[:-1] + ');'
    # Connect to database and execute statement
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    cur.execute(sqlStatement)

    # Write all log data to a list of rows
    data = []
    for i in range(0,len(logComp.logData.timeStamp)):
        dataRow = [logComp.logData.timeStamp[i], logComp.logData.time[i]]
        for dataList in logComp.logData.rawData:
            dataRow.append(dataList[i])
        for dataList in logComp.logData.convData:
            dataRow.append(dataList[i])
        data.append(tuple(dataRow))

    tablename = "data" + str(logComp.id)
    rowNum = ('?,' * len(headerList))[:-1]
    # SQL statement to insert log data into new data table
    cur.executemany('INSERT INTO ' + tablename + ' VALUES(' + rowNum + ')',data)
    conn.commit()
    conn.close()


# Write new log metadata to the main database table
# (Objectives 5.2)
def WriteLog(newLog):
    global database
    valuesList = [newLog.name, newLog.date, newLog.time, newLog.loggedBy, newLog.downloadedBy]
    sql_insert_metadata = """INSERT INTO main (name, date, time, logged_by, downloaded_by)
                                        VALUES(?,?,?,?,?);"""
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    cur.execute(sql_insert_metadata, valuesList)
    conn.commit()
    conn.close()


# Create new config table and write config data to database
# (Objective 5.2)
def WriteConfig(config):
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    rowList = []
    # Write data for each Pin to a row
    for pin in config.pinList:
        row = []
        row.append(pin.id)
        row.append(pin.name)
        row.append(pin.enabled)
        row.append(pin.fName)
        row.append(pin.inputType)
        row.append(pin.gain)
        row.append(pin.scaleMin)
        row.append(pin.scaleMax)
        row.append(pin.units)
        row.append(pin.m)
        row.append(pin.c)
        rowList.append(tuple(row))
    # Get recent id and use it to create new config table
    id = GetRecentId()
    configName = 'config' + id
    sql_create_config = """CREATE TABLE """ + configName + """ (
                                    id INTEGER NOT NULL PRIMARY KEY,
                                    name TEXT NOT NULL,
                                    enabled BOOL NOT NULL,
                                    fName TEXT,
                                    inputType TEXT,
                                    gain INTEGER,
                                    scaleMin REAL,
                                    scaleMax REAL,
                                    units TEXT,
                                    m REAL,
                                    c REAL);"""
    # Insert values into new table
    sql_insert_config = """INSERT INTO """ + configName + """ VALUES(?,?,?,?,?,?,?,?,?,?,?);"""
    cur.execute(sql_create_config)
    cur.executemany(sql_insert_config, rowList)
    conn.commit()
    conn.close()


# Gets the Id for the most recent log from the database
def GetRecentId():
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    # Most recent id will be the largest as it increments for each new log
    cur.execute("SELECT MAX(id) FROM main;")
    logId = cur.fetchone()[0]
    # If no logs are in the database, through an error
    # Error is caught by code which called GetRecentId()
    if logId == None:
        raise ValueError
        return
    conn.close()
    return str(logId)


# Gets the most recent log meta data
# (Objective 8)
def GetRecentMetaData():
    id = GetRecentId()
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    row = cur.execute("SELECT * FROM main WHERE id = ?;",[id]).fetchone()
    # If no log exists, throw error which is caught
    if row == []:
        raise ValueError
        return
    logMeta = lgOb.LogMeta()
    logMeta.id = row[0]
    logMeta.name = row[1]
    logMeta.date = row[2]
    logMeta.time = row[3]
    logMeta.loggedBy = row[4]
    logMeta.downloadedBy = row[5]
    conn.close()
    return logMeta


# Gets the time interval of the most recent log
# (Objective 2.1)
def GetRecentInterval():
    id = GetRecentId()
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    # Retrieve time interval from row with most recent id
    logInterval = cur.execute("SELECT time FROM main WHERE id = ?;", id).fetchone()
    return logInterval[0]


# Gets the most recent config settings from database
# (Objective 2.1)(
def GetRecentConfig():
    id = GetRecentId()
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    # Set table name using most recent id
    configName = "config" + id
    cur.execute("SELECT * FROM " + configName + ";")
    data = cur.fetchall()
    config = lgOb.ConfigFile()
    # Write each row from the table to a Pin object
    for row in data:
        newPin = lgOb.Pin()
        newPin.id = row[0]
        newPin.name = row[1]
        newPin.enabled = bool(row[2])
        newPin.fName = row[3]
        newPin.inputType = row[4]
        newPin.gain = row[5]
        newPin.scaleMin = row[6]
        newPin.scaleMax = row[7]
        newPin.units = row[8]
        newPin.m = row[9]
        newPin.c = row[10]
        config.pinList.append(newPin)
    conn.close()
    return config


# Finds logs not downloaded by the user
# (Objective 3.1)
def FindNotDownloaded(user):
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    # Searches for logs where the username is not contained in the downloaded_by field
    cur.execute("SELECT id, name, date FROM main WHERE downloaded_by NOT LIKE \'%" + user + "%\'")
    logs = cur.fetchall()
    # If there are no logs to be downloaded, throw error which is caught
    if logs == []:
        raise ValueError
        return
    conn.close()
    return logs


# Sets a log to downloaded once a user has downloaded or had the chance to download it
# (Objective 3)
def SetDownloaded(id,user):
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    # Update row to include the username in the downloaded_by field
    cur.execute("UPDATE main SET downloaded_by = downloaded_by || \',\' || ? WHERE id = ?",[user,str(id)])
    conn.commit()
    conn.close()


# Check if a data table already exists for a log
# Used if log has been started without uploading a new config file
# (Objective 7)
def CheckDataTable(id):
    table_name = "data" + id
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    # Returns the number of tables in the database with the name <table_name>
    table_exists = cur.execute("SELECT count(*) FROM sqlite_master WHERE type=\'table\' AND name=?;",[table_name]).fetchone()
    conn.close()
    if table_exists[0] == 0:
        return False
    else:
        return True


# Checks if a log name has already been used
# (Objective 5)
def CheckName(name):
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    # Returns the number of logs with the name <name>
    name_exists = cur.execute("SELECT count(*) FROM main WHERE name = ?", [name]).fetchone()
    if name_exists[0] == 0:
        return False
    else:
        return True
    conn.close()


# Reads full log from the database
# (Objective 4.3)
def ReadLog(id):
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    logMeta = lgOb.LogMeta()
    # Get the metadata for the log from main table
    row = cur.execute('SELECT * FROM main WHERE id = ?',str(id)).fetchone()
    logMeta.id = row[0]
    logMeta.name = row[1]
    logMeta.date = row[2]
    logMeta.time = row[3]
    logMeta.loggedBy = row[4]
    logMeta.downloadedBy = row[5]
    # Get config data for log
    logMeta.config = ReadConfig(id)
    # Get logged data for log
    logMeta.logData = ReadLogData(id)
    conn.close()
    return logMeta


# Returns the time interval for a log
# (Objective 6.3)
def ReadInterval(id):
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    # Retrieves the time interval for a log
    interval = cur.execute("SELECT time FROM main WHERE id = ?",id).fetchone()[0]
    # If no time interval is found, throw error which is caught
    if interval == None:
        raise ValueError
        return
    conn.close()
    return interval


# Reads all config settings from config table
# (Objectives 4.3 and 6.3)
def ReadConfig(id):
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    logConfig = lgOb.ConfigFile()
    configName = "config" + str(id)
    # Retrieves all rows from config table
    config = cur.execute("SELECT * FROM " + configName + ";").fetchall()
    # Converts each row into a Pin object
    for row in config:
        newPin = lgOb.Pin()
        newPin.id = row[0]
        newPin.name = row[1]
        newPin.enabled = bool(row[2])
        newPin.fName = row[3]
        newPin.inputType = row[4]
        newPin.gain = row[5]
        newPin.scaleMin = row[6]
        newPin.scaleMax = row[7]
        newPin.units = row[8]
        newPin.m = row[9]
        newPin.c = row[10]
        logConfig.pinList.append(newPin)
    conn.close()
    return logConfig


# Reads all log data from a log
# (Objectives 4.3)
def ReadLogData(id):
    global database
    logData = lgOb.LogData()
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    dataName = "data" + str(id)
    # Retrieves all rows from the database table
    data = cur.execute("SELECT * FROM " + dataName + ";").fetchall()
    # Creates the lists inside the rawData and convData objects
    # The number of lists being created is half of the number of columns - 1
    logData.InitRawConv(int(len(data[0])/2 - 1))
    for row in data:
        logData.timeStamp.append(row[0])
        logData.time.append(row[1])
        # Array indexing used to pick out the raw and conv data columns from the row
        rawData = row[2:int((len(row)/2) + 1)]
        logData.AddRawData(rawData)
        convData = row[int((len(row)/2) + 1):len(row)]
        logData.AddConvData(convData)
    conn.close()
    return logData


# Updates the date on the log to be true to when the log was started
# (Objective 10)
def AddDate(timestamp,id):
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    # Updates date of log to the current date when log was started
    cur.execute("UPDATE main SET date = ? WHERE id = ?", [timestamp,str(id)])
    conn.commit()
    conn.close()
    return


# Searches for a log using arguments specified by user
# (Objectives 4.1 and 6.1)
def SearchLog(args):
    global database
    conn = sqlite3.connect(database)
    cur = conn.cursor()
    sql = "SELECT * FROM main WHERE "
    # If there are no arguments specified, return all logs
    if args == {}:
        sql = "SELECT * FROM main"
        logs = cur.execute(sql).fetchall()
    else:
        values = []
        # Uses args dictionary to dynamically generate SQL query
        for key in args.keys():
            if key == "date":
                sql += key + " LIKE ? AND "
            else:
                sql += key + " = ? AND "
            values.append(args[key])
        # Remove trailing "AND " from query
        sql = sql[:-4]
        # Fetch all logs that match query
        logs = cur.execute(sql,values).fetchall()
    # If no logs found, throw error which is caught
    if logs == []:
        raise ValueError
        return
    conn.close()
    return logs