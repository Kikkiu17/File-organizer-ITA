def wdata(towrite):
    with open(r"C:\ProgramData\File Organizer\np.txt", "w") as f:
        f.write(towrite)

def wconfig(towrite):
    with open(r"C:\ProgramData\File Organizer\config.txt", "w") as f:
        f.write(towrite)