import pandas as pd
import sys
from pathlib import Path

# Get path of appData folder from argument
path = Path(sys.argv[1])

# Read in data from temp.csv
current_data = pd.read_csv(path / "temp.csv")
# Get the number of points to be used in each average
avg_num = input("Please enter the number of points to be averaged each time.\n>")

# Make sure user inputs an integer
while (type(avg_num) != int):
    try:
        avg_num = int(avg_num)
    except ValueError:
        print("Please input an integer value.")
        avg_num = input("Please enter the number of points to be averaged each time.\n>")
        avg_num = int(avg_num)

# Create DataFrame for processed data
proc_data = pd.DataFrame(columns=current_data.columns)

for i in range(0,len(current_data.index)):
    temp_row = []
    temp_row.append(current_data['Date/Time'][i])
    temp_row.append(current_data['Time (seconds)'][i])
    # Calculate mean for each column
    # Objective 19
    for column in current_data.columns[2:]:
        mean = 0
        # If the end of the data is reached, decrease the average number by 1
        # This avoids an OutOfRange exception
        if(i + avg_num > len(current_data.index)):
            avg_num -= 1
        for j in range(0,avg_num):
            mean += current_data[column][i + j]
        mean = mean / avg_num
        temp_row.append(mean)
    # Add new row to processed data
    temp_row = pd.Series(temp_row, index=current_data.columns,name=str(i))
    proc_data = proc_data.append(temp_row)

# Write processed data to csv
proc_data.to_csv("proc.csv",index=False)