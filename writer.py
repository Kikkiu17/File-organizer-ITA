import time
def wdata(towrite):
    time.sleep(1)
    with open(r"C:\ProgramData\File Organizer\np.txt", "w") as f:
        f.write(towrite)

def wconfig(towrite):
    time.sleep(1)
    with open(r"C:\ProgramData\File Organizer\config.txt", "w") as f:
        f.write(towrite)