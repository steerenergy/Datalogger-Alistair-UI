import pandas as pd
import matplotlib.pyplot as plt
import sys
from pathlib import Path

# Get path of appData folder from argument
path = Path(sys.argv[1])

# Read in data from temp.csv
current_data = pd.read_csv(path / "temp.csv")

# Output available columns to user
print("Columns: ")
# Store columns in a dictionary with a number to reference each column
col_dict = dict()
i = 0
for column in current_data.columns:
    col_dict[i] = column
    print(str(i) + ": " + column)
    i += 1

# Get the number (index) of the X axis column
x_col_i = input("Input X-axis index.\n>")
# Make sure user inputs a valid index
while (type(x_col_i) != int):
    try:
        x_col_i = int(x_col_i)
        while x_col_i not in col_dict.keys():
            print("That column does not exist.")
            x_col_i = input("Input X-axis index.\n>")
            x_col_i = int(x_col_i)
    except ValueError:
        print("Please input an integer value.")
        x_col_i = input("Input X-axis index.\n>")
# Set the X axis column to the column corresponding to the entered index
x_col = col_dict[x_col_i]

# Get the number (index) of the Y axis column
y_col_i = input("Input Y-axis index.\n>")
# Make sure user inputs a valid index
while (type(y_col_i) != int):
    try:
        y_col_i = int(y_col_i)
        while y_col_i not in col_dict.keys():
            print("That column does not exist.")
            y_col_i = input("Input Y-axis index.\n>")
            y_col_i = int(y_col_i)
    except ValueError:
        print("Please input an integer value.")
        y_col_i = input("Input Y-axis index.\n>")
# Set the Y axis column to the column corresponding to the entered index
y_col = col_dict[y_col_i]

# Get title for graph
title = input("Input graph title.\n>")

# Create new figure and axes
fig, ax = plt.subplots()
# Plot X column against Y column
ax.plot(current_data[x_col], current_data[y_col])

# Set the axis labels and title
ax.set(xlabel=x_col,ylabel=y_col,title=title)
ax.grid()

# Show the graph to user
# Objectives 16.2 and 21
plt.show()
# User can save graph using Matplotlib display window
# Objective 16.3