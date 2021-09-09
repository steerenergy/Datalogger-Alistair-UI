import pandas as pd
import sys
from pathlib import Path

# Get path of appData folder from argument
path = Path(sys.argv[1])

# Read in data from temp.csv
current_data = pd.read_csv(path / "temp.csv")

proc_columns = []
drop_columns = []
# Enumerate through data columns and allow user to select whether to apply the moving average
for column in list(current_data)[2:]:
    apply = input("Apply moving average to " + column + "?\n[y\\n]:")
    if apply.lower() == "y":
        proc_columns.append(column)
    else:
        drop_columns.append(column)

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
proc_data = current_data

# Apply moving_average to each column selected to be processed
for column in proc_columns:
    proc_data[column] = proc_data[column].rolling(min_periods=1,window=avg_num).mean()
# Write data to csv
proc_data.to_csv("proc.csv",index=False)
