import pandas as pd
from decimal import Decimal
from decimal import InvalidOperation
import sys
from pathlib import Path

# Get path of appData folder from argument
path = Path(sys.argv[1])#

# Read data in from csv file
current_data = pd.read_csv(path / "temp.csv")
# Get m value of y = mx + c function
m = input("Please enter the m value.\n>")

# Make sure user inputs a valid decimal value for m
while (type(m) != Decimal):
    try:
        m = Decimal(m)
    except InvalidOperation:
        print("Please input a decimal value.")
        m = input("Please enter the m value.\n>")
        m = Decimal(m)

# Get c value of y = mx + c function
c = input("Please enter the c value.\n>")

# Make sure user inputs a valid decimal value for c
while (type(c) != Decimal):
    try:
        c = Decimal(c)
    except InvalidOperation:
        print("Please input a decimal value.")
        c = input("Please enter the c value.\n>")
        c = Decimal(c)

proc_columns = current_data.columns[0:2]
# Enumerate through data columns and allow user to select whether to apply the linear function
for column in current_data.columns[2:]:
    apply = input("Apply linear function to " + column + "?\n[y\\n]:")
    if apply.lower() == "y":
        proc_columns = proc_columns.append(pd.Index([column]))

# Create DataFrame for processed data
proc_data = pd.DataFrame(columns=proc_columns)

for i in range(0,len(current_data.index)):
    temp_row = []
    temp_row.append(current_data['Date/Time'][i])
    temp_row.append(current_data['Time (seconds)'][i])
    # Apply linear function to data in selected columns
    # Objective 20
    for column in proc_columns[2:]:
        value = (Decimal(current_data[column][i]) * m) + c
        temp_row.append(f"{value:.14f}")
    # Append new row to processed data
    temp_row = pd.Series(temp_row, index=proc_columns,name=str(i))
    proc_data = proc_data.append(temp_row)

# Write processed data to csv
proc_data.to_csv("proc.csv",index=False)

