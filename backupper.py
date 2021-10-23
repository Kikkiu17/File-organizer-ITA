    
def backup(datetime, backfolder, new_type_list, new, groups, direc, stat, assoluta, isbfolder):
    print(backfolder, isbfolder)
    import os
    import shutil
    rawdata = datetime.now()
    datat = rawdata.strftime("%d/%m/%Y %H:%M:%S")
    finaldata = datat.replace("/", "-").replace(":", ".")
    print("C: not in",str(backfolder))
    if("C:" not in str(backfolder)):
        backfolder = assoluta
        halt = 0
        for oggetto in os.listdir(backfolder):
            if("backups" in oggetto.lower()):
                if("backups" == oggetto.lower()):
                    halt = halt + 1
        if(halt == 0):
            os.mkdir(backfolder+"\\Backups")
        os.mkdir(backfolder+"\\Backups\\"+str(finaldata))
        for group_type in new_type_list.split("\n"):
            if(group_type.endswith(" ")):
                li = group_type.rsplit(" ", 1)
                group_type = new.join(li)
            if(group_type != ""):
                for group_num in range(len(groups[group_type])):
                    selected_file = groups[group_type][group_num]
                    check_final_dir = os.listdir(backfolder+"\\Backups\\"+str(finaldata))
                    if(selected_file not in check_final_dir):
                        shutil.copy(direc+selected_file, backfolder+"\\Backups\\"+str(finaldata))
    else:
        halt = 0
        for oggetto in os.listdir(backfolder):
            if("backups" in oggetto.lower()):
                if("backups" == oggetto.lower()):
                    halt = halt + 1
        if(halt == 0):
            os.mkdir(backfolder+"\\Backups")
        os.mkdir(backfolder+"\\Backups\\"+str(finaldata))
        for group_type in new_type_list.split("\n"):
            if(group_type.endswith(" ")):
                li = group_type.rsplit(" ", 1)
                group_type = new.join(li)
            if(group_type != ""):
                for group_num in range(len(groups[group_type])):
                    selected_file = groups[group_type][group_num]
                    check_final_dir = os.listdir(backfolder+"\\Backups\\"+str(finaldata))
                    if(selected_file not in check_final_dir):
                        shutil.copy(direc+selected_file, backfolder+"\\Backups\\"+str(finaldata))

def delbackup(datetime, backfolder, new_type_list, new, groups, direc, stat, assoluta, isbfolder):
    print(backfolder, isbfolder)
    import os
    import shutil
    rawdata = datetime.now()
    datat = rawdata.strftime("%d/%m/%Y %H:%M:%S")
    finaldata = datat.replace("/", "-").replace(":", ".")
    print("C: not in",str(backfolder))
    if("C:" not in str(backfolder)):
        backfolder = assoluta
        print(backfolder)
        for root, dirs, files in os.walk(backfolder+"\\Backups"):
            for fname in files:
                full_path = os.path.join(root, fname)
                os.chmod(full_path ,stat.S_IWRITE)
        halt = 0
        for oggetto in os.listdir(backfolder):
            if("backups" in oggetto.lower()):
                if("backups" == oggetto.lower()):
                    halt = halt + 1
        if(halt == 0):
            shutil.rmtree(backfolder+"\\Backups")
        os.mkdir(backfolder+"\\Backups\\")
        os.mkdir(backfolder+"\\Backups\\"+str(finaldata))
        for group_type in new_type_list.split("\n"):
            if(group_type.endswith(" ")):
                li = group_type.rsplit(" ", 1)
                group_type = new.join(li)
            if(group_type != ""):
                for group_num in range(len(groups[group_type])):
                    selected_file = groups[group_type][group_num]
                    check_final_dir = os.listdir(backfolder+"\\Backups\\"+str(finaldata))
                    if(selected_file not in check_final_dir):
                        shutil.copy(direc+selected_file, backfolder+"\\Backups\\"+str(finaldata))
    else:
        for root, dirs, files in os.walk(backfolder+"\\Backups"):
            for fname in files:
                full_path = os.path.join(root, fname)
                os.chmod(full_path ,stat.S_IWRITE)
        halt = 0
        for oggetto in os.listdir(backfolder):
            if("backups" in oggetto.lower()):
                if("backups" == oggetto.lower()):
                    halt = halt + 1
        if(halt == 0):
            shutil.rmtree(backfolder+"\\Backups")
        os.mkdir(backfolder+"\\Backups\\")
        os.mkdir(backfolder+"\\Backups\\"+str(finaldata))
        for group_type in new_type_list.split("\n"):
            if(group_type.endswith(" ")):
                li = group_type.rsplit(" ", 1)
                group_type = new.join(li)
            if(group_type != ""):
                for group_num in range(len(groups[group_type])):
                    selected_file = groups[group_type][group_num]
                    check_final_dir = os.listdir(backfolder+"\\Backups\\"+str(finaldata))
                    if(selected_file not in check_final_dir):
                        shutil.copy(direc+selected_file, backfolder+"\\Backups\\"+str(finaldata))