import pandas as pd
from decimal import Decimal

current_data = pd.read_csv("temp.csv")
m = input("Please enter the m value.\n>")

while (type(m) != Decimal):
    try:
        m = Decimal(m)
    except ValueError:
        print("Please input a decimal value.")
        m = input("Please enter the m value.\n>")
        m = Decimal(m)

c = input("Please enter the c value.\n>")

while (type(c) != Decimal):
    try:
        c = Decimal(c)
    except ValueError:
        print("Please input a decimal value.")
        c = input("Please enter the m value.\n>")
        c = Decimal(c)

proc_columns = current_data.columns[0:2]
for column in current_data.columns[2:]:
    apply = input("Apply linear function to " + column + "?\n[y\\n]:")
    if apply.lower() == "y":
        proc_columns = proc_columns.append(pd.Index([column]))

proc_data = pd.DataFrame(columns=proc_columns)

for i in range(0,len(current_data.index)):
    temp_row = []
    temp_row.append(current_data['Date/Time'][i])
    temp_row.append(current_data['Time (seconds)'][i])
    for column in proc_columns[2:]:
        value = (Decimal(current_data[column][i]) * m) + c
        temp_row.append(value)
    temp_row = pd.Series(temp_row, index=proc_columns,name=str(i))
    proc_data = proc_data.append(temp_row)

proc_data.to_csv("proc.csv",index=False)

