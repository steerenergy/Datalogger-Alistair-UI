import pandas as pd
import matplotlib.pyplot as plt

current_data = pd.read_csv("temp.csv")

print("Columns: ")
col_dict = dict()
i = 0
for column in current_data.columns:
    col_dict[i] = column
    print(str(i) + ": " + column)
    i += 1

x_col_i = input("Input X-axis index.\n>")
while (type(x_col_i) != int):
    try:
        x_col_i = int(x_col_i)
        while x_col_i not in col_dict.keys():
            print("That column name does not exist.")
            x_col = input("Input X-axis column name.\n>")
    except ValueError:
        print("Please input an integer value.")
        x_col_i = input("Input X-axis index.\n>")
x_col = col_dict[x_col_i]

y_col_i = input("Input Y-axis index.\n>")
while (type(y_col_i) != int):
    try:
        y_col_i = int(y_col_i)
        while y_col_i not in col_dict.keys():
            print("That column name does not exist.")
            y_col = input("Input Y-axis column name.\n>")
    except ValueError:
        print("Please input an integer value.")
        y_col_i = input("Input Y-axis index.\n>")
y_col = col_dict[y_col_i]

title = input("Input graph title.\n>")

fig, ax = plt.subplots()
ax.plot(current_data[x_col], current_data[y_col])

ax.set(xlabel=x_col,ylabel=y_col,title=title)
ax.grid()

plt.show()
