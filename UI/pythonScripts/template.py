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

# Stores which columns will be processed and which will be dropped
proc_columns = []
drop_columns = []
# Enumerate through data columns and allow user to select whether to apply the mathematical function
for column in list(current_data):
    apply = input("Apply mathematical function to " + column + "?\n[y\\n]:")
    if apply.lower() == "y":
        proc_columns.append(column)
    else:
        drop_columns.append(column)

""" 
! If the function requires input, e.g. a variable such as m or c, take input here
! Example: m = Decimal(input("Please input a value for m"))
"""

# Create DataFrame for processed data, drop columns that won't be processed
proc_data = current_data.drop(columns=drop_columns,axis=1)

# Apply mathematical function to each column selected to be processed
for column in proc_columns:
    """
    ! To apply the function, use proc_data[column] = proc_data[column].apply() and place the function in apply
    ! If the function is simple, a lambda expression can be used
    ! For example, you may use this: 'proc_data[column] = proc_data[column].apply(lambda x : Decimal(x) * m + c)' to apply a linear function
    """

# Write processed data to csv
proc_data.to_csv("proc.csv",index=False)
