import pandas as pd
from decimal import Decimal
from decimal import InvalidOperation
import sys
from pathlib import Path

"""
This file serves as a template for performing a mathematical operation to specific columns of data
Edit the sections marked with ! to change the operation it is performing
"""

# Get path of appData folder from argument
path = Path(sys.argv[1])

# Read data in from csv file into a dataframe
current_data = pd.read_csv(path / "temp.csv")

# Pulls the data columns from the dataframe
proc_columns = current_data.columns[0:2]
# Enumerate through data columns and allow user to select whether to apply the mathematical function
for column in current_data.columns[2:]:
    apply = input("Apply linear function to " + column + "?\n[y\\n]:")
    if apply.lower() == "y":
        proc_columns = proc_columns.append(pd.Index([column]))

""" 
! If the function requires input, e.g. a variable such as m or c, take input here
! Example: m = input("Please input a value for m")
"""

# Create DataFrame for processed data
proc_data = pd.DataFrame(columns=proc_columns)

# Iterate through dataframe and apply function to select columns
for i in range(0,len(current_data.index)):
    temp_row = []
    temp_row.append(current_data['Date/Time'][i])
    temp_row.append(current_data['Time (seconds)'][i])
    # Apply linear function to data in selected columns
    for column in proc_columns[2:]:
        """
        ! Apply mathematical function here
        ! Example: value = Decimal(current_data[column][i]) ^ 2 would square the values
        ! Use "Decimal(current_data[column[i]) to get the value to apply the function to
        """
        temp_row.append(f"{value:.14f}")
    # Append new row to processed data
    temp_row = pd.Series(temp_row, index=proc_columns,name=str(i))
    proc_data = proc_data.append(temp_row)

# Write processed data to csv
proc_data.to_csv("proc.csv",index=False)
