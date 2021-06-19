import pandas as pd

current_data = pd.read_csv("temp.csv")
avg_num = input("Please enter the number of points to be averaged each time.\n>")

while (type(avg_num) != int):
    try:
        avg_num = int(avg_num)
    except ValueError:
        print("Please input an integer value.")
        avg_num = input("Please enter the number of points to be averaged each time.\n>")
        avg_num = int(avg_num)

proc_data = pd.DataFrame(columns=current_data.columns)

for i in range(0,len(current_data.index)):
    temp_row = []
    temp_row.append(current_data['Date/Time'][i])
    temp_row.append(current_data['Time (seconds)'][i])
    for column in current_data.columns[2:]:
        mean = 0
        if(i + avg_num > len(current_data.index)):
            avg_num -= 1
        for j in range(0,avg_num):
            mean += current_data[column][i + j]
        mean = mean / avg_num
        temp_row.append(mean)
    temp_row = pd.Series(temp_row, index=current_data.columns,name=str(i))
    proc_data = proc_data.append(temp_row)

proc_data.to_csv("proc.csv",index=False)