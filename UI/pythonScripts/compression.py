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
proc_data = current_data

# Used to make dataframe size divisible by comp_ratio
n = 0
while (len(proc_data) - n) % comp_ratio != 0:
    n += 1
# remove last n rows
proc_data.drop(proc_data.index[-n:],inplace=True)

# Calculate forward moving average for all data columns
indexer = pd.api.indexers.FixedForwardWindowIndexer(window_size=comp_ratio)
for column in list(proc_data)[2:]:
    proc_data[column] = proc_data[column].rolling(window=indexer, min_periods=comp_ratio).mean()

# Remove rows of data using compression ratio
passes = len(proc_data) - comp_ratio + 1
for i in range(0,passes, comp_ratio):
    for j in range(1,comp_ratio):
        proc_data.drop(index=[i + j],axis=0,inplace=True)

# Write processed data to csv
proc_data.to_csv("proc.csv",index=False)
