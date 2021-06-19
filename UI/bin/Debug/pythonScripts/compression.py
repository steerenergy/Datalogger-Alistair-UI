import pandas as pd

current_data = pd.read_csv("temp.csv")
comp_ratio = input("Please input how many values to compress to 1?\n>")

while (type(comp_ratio) != int):
    try:
        comp_ratio = int(comp_ratio)
    except ValueError:
        print("Please input an integer value.")
        comp_ratio = input("Please input how many values to compress to 1?\n>")
        comp_ratio = int(comp_ratio)

proc_data = pd.DataFrame(columns=current_data.columns)

i = 0
while i <= (current_data['Date/Time'].count() - comp_ratio):
    temp_row = []
    temp_row.append(current_data['Date/Time'][i])
    temp_row.append(current_data['Time (seconds)'][i])
    for column in current_data.columns[2:]:
        mean = 0
        for j in range(0,comp_ratio):
            mean += current_data[column][i + j]
        mean = mean / comp_ratio
        temp_row.append(mean)
    temp_row = pd.Series(temp_row, index=current_data.columns,name=str(i))
    proc_data = proc_data.append(temp_row)
    i += comp_ratio

proc_data.to_csv("proc.csv",index=False)