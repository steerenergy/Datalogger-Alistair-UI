import pandas as pd
import sys
from pathlib import Path

# Get path of appData folder from argument
path = Path(sys.argv[1])

# Read in data from temp.csv
current_data = pd.read_csv(path / "temp.csv")
# Get the compression ratio from user
comp_ratio = input("Please input how many values to compress to 1?\n>")

# Make sure user inputs an integer value
while (type(comp_ratio) != int):
    try:
        comp_ratio = int(comp_ratio)
    except ValueError:
        print("Please input an integer value.")
        comp_ratio = input("Please input how many values to compress to 1?\n>")
        comp_ratio = int(comp_ratio)

# Create processed data DataFrame
proc_data = pd.DataFrame(columns=current_data.columns)

i = 0
while i <= (current_data['Date/Time'].count() - comp_ratio):
    temp_row = []
    temp_row.append(current_data['Date/Time'][i])
    temp_row.append(current_data['Time (seconds)'][i])
    # Calculate mean value for each column of data
    for column in current_data.columns[2:]:
        mean = 0
        for j in range(0,comp_ratio):
            mean += current_data[column][i + j]
        mean = mean / comp_ratio
        temp_row.append(mean)
    # Append compressed row to proc_data DataFrame
    temp_row = pd.Series(temp_row, index=current_data.columns,name=str(i))
    proc_data = proc_data.append(temp_row)
    # Increment i by compression ratio
    i += comp_ratio

# Write processed data to csv
proc_data.to_csv("proc.csv",index=False)