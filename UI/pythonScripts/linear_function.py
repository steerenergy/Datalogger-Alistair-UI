import pandas as pd
from decimal import Decimal
from decimal import InvalidOperation
import sys
from pathlib import Path

# Get path of appData folder from argument
path = Path(sys.argv[1])

# Read data in from csv file
current_data = pd.read_csv(path / "temp.csv")

proc_columns = []
drop_columns = []
# Enumerate through data columns and allow user to select whether to apply the linear function
for column in list(current_data)[2:]:
    apply = input("Apply linear function to " + column + "?\n[y\\n]:")
    if apply.lower() == "y":
        proc_columns.append(column)
    else:
        drop_columns.append(column)

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

# Create DataFrame for processed data, drop columns that won't be processed
proc_data = current_data.drop(columns=drop_columns,axis=1)

# Apply linear function to each column selected to be processed
for column in proc_columns:
    proc_data[column] = proc_data[column].apply(lambda x : Decimal(x) * m + c)
# Write data to csv
proc_data.to_csv("proc.csv",index=False)
