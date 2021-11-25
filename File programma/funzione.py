from os import replace
from urllib.request import ftpwrapper


def copy_move(directory, configfile, datafile, optype, backup, delbackup, backfolder, assoluta, isbfolder):
    import shutil
    import os
    from os.path import dirname, abspath
    import magic
    import requests
    import json
    import win32com.client
    import inflect
    import stat
    from datetime import datetime
    import writer
    import backupper
    import logger
    import winreg
    if("np.txt" not in os.listdir(r"C:\ProgramData\File Organizer")):
        with open(r"C:\ProgramData\File Organizer\np.txt", "w") as f:
            f.write('{"name":"null", "pdir":"null", "bfolder":"null"}')
    firma = "################################################################\n###   ####   ###  ###   ####   ###   ####   ###  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###  #####  ###\n###   ##   #####  ###   ##   #####   ##   #####  ###  #####  ###\n###       ######  ###       ######       ######  ###  #####  ###\n###   ##   #####  ###   ##   #####   ##   #####  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###         ###\n#################################################versione:3.7.0#"
    print(firma)
    direc = directory
    with open("C:\ProgramData\File Organizer\config.txt") as f:
        lines = f.readlines()
    with open("C:\ProgramData\File Organizer\\np.txt") as f:
        dates = f.readlines()
    configdict = json.loads(lines[0])
    datadict = json.loads(dates[0])
    #nome_pdir = lines[1].split(":")[1]
    conf_dir = configdict["dir"]
    main_dir = os.listdir(direc)
    lista_file = os.listdir(direc)
    type_list = ""
    groups = {}
    main_dir_check = 0
    num_of_created_dirs = 0
    output = ""
    towrite = ""
    errore = ""
    log = ""
    log_var = ""
    new = ""
    reg_ext_list = ""
    rel_num = 0

    for x in range(0, winreg.QueryInfoKey(winreg.HKEY_CLASSES_ROOT)[0]):
        reg_ext = winreg.EnumKey(winreg.HKEY_CLASSES_ROOT, x)
        if(reg_ext_list == ""):
            rel_num = 0
            reg_ext_list = "0 " + reg_ext
        else:
            rel_num = rel_num + 1
            reg_ext_list = reg_ext_list + "\n" + str(rel_num) + " " + reg_ext

    for file in main_dir:
        if(file == conf_dir):
            main_dir_check = main_dir_check+1
        elif(file == str(conf_dir).lower()):
            main_dir_check = main_dir_check+1
    # #fordev_print(main_dir_check)
    create_main_dir_check = 0
    if(main_dir_check == 0):
        num_of_created_dirs = num_of_created_dirs+1
        if(datadict["name"] == "null"):
            to_log = "datadict[\"name\"] == null"
            to_log_var = datadict["name"]+" == null"
            log_out = logger.b_log(to_log, to_log_var, log, log_var)
            log = log_out[0]
            log_var = log_out[1]
            #fordev_print(to_log)
            try:
                os.mkdir(direc+conf_dir)
            except Exception as err:
                to_log = str(str(err))
                log = log = logger.n_log(to_log, log)
                if(errore == ""):
                    errore = str(str(err))
                else:
                    errore = errore+"\n" + \
                        str(str(err))
            if(str(conf_dir).lower() in direc):
                to_log = str(err)
                log = logger.n_log(to_log, log)
                #fordev_print(to_log)
                try:
                    os.rename(str(conf_dir).lower(), conf_dir)
                except Exception as err:
                    to_log = str(err)
                    log = logger.n_log(to_log, log)
                    if(errore == ""):
                        errore = str(err)
                    else:
                        errore = errore+"\n" + \
                            str(err)
            datadict["name"] = conf_dir
            datadict["pdir"] = assoluta
            out = f">>> Benvenuto <<<\n>>> Cartella creata: {direc}{conf_dir}\n"
            output = str(output) + str(out)
            writer.wdata(json.dumps(datadict))
            create_main_dir_check = create_main_dir_check+1
        else:
            to_log = "datadict[\"name\"] != null"
            to_log_var = datadict["name"]+" == null"
            #fordev_print(to_log)
            log_out = logger.b_log(to_log, to_log_var, log, log_var)
            log = log_out[0]
            log_var = log_out[1]
            is_main_dir_present = 0
            for file in main_dir:
                if("." not in file):
                    if((file == datadict["name"] and file != conf_dir) or (file.lower() == str(datadict["name"]).lower() and file.lower() != str(conf_dir).lower())):
                        to_log = "\".\" not in file ---AND--- (file == datadict[\"name\"] and file != conf_dir) or (file.lower() == str(datadict[\"name\"]).lower() and file.lower() != str(conf_dir).lower())"
                        to_log_var = file, datadict["name"], conf_dir
                        log_out = logger.b_log(
                            to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                        #fordev_print(to_log)
                        is_main_dir_present = is_main_dir_present+1
            if(is_main_dir_present > 0):
                to_log = "is_main_dir_present > 0"
                to_log_var = is_main_dir_present
                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                log = log_out[0]
                log_var = log_out[1]
                #fordev_print(to_log)
                torename = direc+datadict["name"]
                try:
                    os.rename(direc+datadict["name"], direc+conf_dir)
                except Exception as err:
                    to_log = str(err)
                    log = logger.n_log(to_log, log)
                    if(errore == ""):
                        errore = str(err)
                    else:
                        errore = errore+"\n" + \
                            str(err)
                out = f">>> Cartella rinominata da", datadict["name"], "a", conf_dir+"\n"
                output = str(output) + str(out)
                datadict["name"] = conf_dir
                datadict["pdir"] = assoluta
                writer.wdata(json.dumps(datadict))
                create_main_dir_check = create_main_dir_check+1
            else:
                to_log = "is_main_dir_present =< 0"
                log = logger.n_log(to_log, log)
                #fordev_print(to_log)
                try:
                    os.mkdir(direc+conf_dir)
                except Exception as err:
                    to_log = str(err)
                    logger.n_log(to_log, log)
                    if(errore == ""):
                        errore = str(err)
                    else:
                        errore = errore+"\n" + \
                            str(err)
                if(str(conf_dir).lower() in direc):
                    to_log = "str(conf_dir).lower() in direc"
                    to_log_var = str(conf_dir).lower(), direc
                    log_out = logger.b_log(to_log, to_log_var, log, log_var)
                    log = log_out[0]
                    log_var = log_out[1]
                    #fordev_print(to_log)
                    os.rename(str(conf_dir).lower(), conf_dir)
                out = f">>> La cartella principale non è presente, ne creo una: {direc+conf_dir}"+"\n"
                output = str(output) + str(out)
                datadict["name"] = conf_dir
                datadict["pdir"] = assoluta
                writer.wdata(json.dumps(datadict))
                create_main_dir_check = create_main_dir_check+1

    for file in lista_file:
        if(file != "desktop.ini" and file != "File Organizer.bat" and ".ini" not in file and ".sys" not in file and "Organizer.py" not in file and "Organizer" not in file and "organizer" not in file):
            if("." in file and os.path.isfile(direc+file) == True):
                metadata = ['Name', 'Size', 'Item type',
                            'Date modified', 'Date created']

                def get_file_metadata(path, filename, metadata):
                    sh = win32com.client.gencache.EnsureDispatch(
                        'Shell.Application', 0)
                    ns = sh.NameSpace(path)
                    file_metadata = dict()
                    item = ns.ParseName(str(filename))
                    for ind, attribute in enumerate(metadata):
                        attr_value = ns.GetDetailsOf(item, ind)
                        if attr_value:
                            file_metadata[attribute] = attr_value
                    return file_metadata
                if ("a" == "a"):
                    to_log = "if pass"
                    to_log_var = "a"
                    log_out = logger.b_log(to_log, to_log_var, log, log_var)
                    log = log_out[0]
                    log_var = log_out[1]
                    #fordev_print(to_log)
                    folder = direc
                    filename = file
                    metadata = ['Name', 'Size', 'Item type',
                                'Date modified', 'Date created']
                    proprietà = get_file_metadata(folder, filename, metadata)
                if(proprietà["Item type"] not in groups):
                    to_log = "proprietà[\"Item type\"] not in groups"
                    to_log_var = proprietà["Item type"], groups
                    log_out = logger.b_log(to_log, to_log_var, log, log_var)
                    log = log_out[0]
                    log_var = log_out[1]
                    #fordev_print(to_log)
                    groups[proprietà["Item type"]] = [file]
                else:
                    to_log = "proprietà[\"Item type\"] in groups"
                    to_log_var = proprietà["Item type"], groups
                    log_out = logger.b_log(to_log, to_log_var, log, log_var)
                    log = log_out[0]
                    log_var = log_out[1]
                    #fordev_print(to_log)
                    groups[proprietà["Item type"]].append(file)
                if(proprietà["Item type"] not in type_list.split("\n")):
                    to_log = "proprietà[\"Item type\"] not in type_list.split(\"n\")"
                    to_log_var = proprietà["Item type"], type_list.split("\n")
                    log_out = logger.b_log(to_log, to_log_var, log, log_var)
                    log = log_out[0]
                    log_var = log_out[1]
                    #fordev_print(to_log)
                    if(type_list == ""):
                        type_list = proprietà["Item type"]
                    else:
                        type_list = type_list+"\n"+proprietà["Item type"]

    file_check = ""
    enname = ""
    mod_type_list = type_list

    for x in range(len(groups)):
        nl = type_list.split("\n")
        if(len(groups[nl[x]]) != 0):
            selection = groups[nl[x]][len(groups[nl[x]])-1]
            if(nl[x].lower() == "file "+selection.split(".")[len(selection.split("."))-1]):
                command = selection.split(".")
                rawcom = os.popen(
                    "assoc ."+command[len(command)-1]).read().split("=")
                if(len(rawcom)-1 != 0):
                    if(file_check == ""):
                        file_check = str(x)
                    else:
                        file_check = file_check+"\n"+str(x)
                    comando = rawcom[len(rawcom)-1].replace(".",
                        " ").replace("\n", "")
                    if("file" in comando):
                        to_log = "\"file\" in comando"
                        to_log_var = file, comando
                        log_out = logger.b_log(
                            to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                        #fordev_print(to_log)
                        charcheck = 0
                        for lettera in comando:
                            if(lettera in command[len(command)-1]):
                                to_log = "lettera in command[len(command)-1]"
                                to_log_var = lettera, command[len(command)-1]
                                log_out = logger.b_log(
                                    to_log, to_log_var, log, log_var)
                                log = log_out[0]
                                log_var = log_out[1]
                                #fordev_print(to_log)
                                charcheck = charcheck+1
                        if(charcheck >= len(command[1])-1):
                            to_log = "charcheck >= len(command[1])-1"
                            to_log_var = charcheck, len(command[1])-1
                            log_out = logger.b_log(
                                to_log, to_log_var, log, log_var)
                            log = log_out[0]
                            log_var = log_out[1]
                            #fordev_print(to_log)
                            mime = magic.Magic(mime=True)
                            tipo_file_raw = mime.from_file(direc+selection)
                            tipo_file = tipo_file_raw.split("/")
                            if("cannot" in str(tipo_file).lower()):
                                if(len(groups[nl[x]]) > 1):
                                    selection = groups[nl[x]][len(groups[nl[x]])-2]
                                    mime = magic.Magic(mime=True)
                                    tipo_file_raw = mime.from_file(direc+selection)
                                    tipo_file = tipo_file_raw.split("/")
                            if(enname == ""):
                                enname = tipo_file[0].capitalize()
                            else:
                                enname = enname+","+tipo_file[0].capitalize()
                    else:
                        to_log = "\"file\" not in comando"
                        to_log_var = file, comando
                        log_out = logger.b_log(
                            to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                        #fordev_print(to_log)
                        if(enname == ""):
                            enname = comando
                        else:
                            enname = enname+","+comando

    #fordev_print(str(type_list.split("\n"))+"\n\n"+str(file_check))
    #fordev_print("\n\n"+enname)

    itaresp = {}
    if(enname != ""):
        to_log = "translate fire"
        #fordev_print(to_log)
        log = logger.n_log(to_log, log)
        p = inflect.engine()
        plurali = ""
        for parola in enname.split(","):
            if("Cannot open" not in str(parola)):
                if(plurali == ""):
                    plurali = p.plural(parola)
                else:
                    plurali = plurali+","+p.plural(parola)
        list_type = ""
        for tipo in mod_type_list.split("\n"):
            if("Cannot open" not in str(tipo)):
                if(list_type == ""):
                    list_type = tipo
                else:
                    list_type = list_type+" ,"+tipo
        to_log_var = plurali, list_type
        if(log_var == ""):
            log_var = "0"+" "+str(to_log_var)+"\n"
        else:
            log_var_len = len(log.split("\n"))
            log_var = log_var+"\n"+str(log_var_len)+" "+str(to_log_var)
        url = "https://microsoft-translator-text.p.rapidapi.com/translate"
        querystring = {"to": "it", "api-version": "3.0",
                       "profanityAction": "NoAction", "textType": "plain"}
        payload = "[\r\n    {\r\n        \"Text\": \"" + \
            plurali+" ,"+list_type+"\"\r\n    }\r\n]"
        headers = {
            'content-type': "application/json",
            'x-rapidapi-host': "microsoft-translator-text.p.rapidapi.com",
            'x-rapidapi-key': "34221de93cmshab1788fe470ce0bp12eebejsndd89b2f16de8"
        }
        response = requests.request(
            "POST", url, data=payload, headers=headers, params=querystring)
        resp_dict = json.loads(response.text)
        if(str(resp_dict) != "{'messages': 'The API is unreachable, please contact the API provider', 'info': 'Your Client (working) ---> Gateway (working) ---> API (not working)'}"):
            itaresp = resp_dict[0]["translations"][0]["text"]
        else:
            itaresp = ""
            #fordev_print("NO ANSWER")

    new_type_list = type_list
    #fordev_print(new_type_list)
    #fordev_print(itaresp)
    #fordev_print(file_check)
    new_split = str(str(itaresp).split(","))
    da_sostituire = ""
    for x in range(len(file_check.split("\n"))):
        resp_split = str(itaresp).split(",")
        # #fordev_print(resp_split)
        nl = file_check.split("\n")
        if(file_check != ''):
            test = type_list.split("\n")[int(nl[x])]
            if(resp_split[x].endswith(" ")):
                li = resp_split[x].rsplit(" ", 1)
                resp_split[x] = new.join(li)
            if(resp_split[x] in groups):
                to_log = "resp_split[x] in groups"
                to_log_var = resp_split[x], groups
                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                log = log_out[0]
                log_var = log_out[1]
                #fordev_print(to_log)
                for tomove in groups[test]:
                    groups[resp_split[x]].append(tomove)
                groups.pop(test, "")
                #fordev_print("NEW TYPE LIST REPLACE FIRST", test)
                if("\n"+test in new_type_list):
                    new_type_list = new_type_list.replace("\n"+test, "")
                elif(test+"\n" in new_type_list):
                    new_type_list = new_type_list.replace(test+"\n", "")
            else:
                to_log = "resp_split[x] not in groups"
                to_log_var = resp_split[x], groups
                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                log = log_out[0]
                log_var = log_out[1]
                #fordev_print(to_log)
                groups[resp_split[x]] = groups.pop(test)
                #fordev_print("NEW TYPE LIST REPLACE SECOND", test, resp_split[x])
                new_type_list = new_type_list.replace(test, resp_split[x])
                new_split = new_split.replace(test, resp_split[x])
                if(da_sostituire == ""):
                    da_sostituire = resp_split[x]+", "
                else:
                    da_sostituire = da_sostituire+resp_split[x]+", "

    file_type = ""

    fileexistcheck = ""
    for file_sel in lista_file:
        if(file_sel != "desktop.ini"):
            if(os.path.isfile(direc+file_sel) == True):
                if(fileexistcheck == ""):
                    fileexistcheck = file_sel+"\n"
                else:
                    fileexistcheck = fileexistcheck+file_sel+"\n"

    # new algorithm --------------------------------------------------------------------------------------------------------------------------
    # controlla se l'estensione del file c'è nel registro
    if(new_type_list != ""):
        if(len(fileexistcheck.split("\n")) >= 1):
            for tipo_f in range(len(new_type_list.split("\n"))):
                sel_obj = new_type_list.split("\n")[tipo_f]
                ext = "."+str(groups[sel_obj][0]).split(".")[
                    len(str(groups[sel_obj][0]).split("."))-1
                ]
                if(ext in reg_ext_list):
                    to_log = "ext in reg_ext_list"
                    to_log_var = ext
                    log_out = logger.b_log(to_log, to_log_var, log, log_var)
                    log = log_out[0]
                    log_var = log_out[1]
                    reg_split = reg_ext_list.split(ext)[0]
                    new_rlist = reg_ext_list
                    reg_ext = new_rlist.replace(reg_split, "").split("\n")[0]
                    key = winreg.OpenKey(winreg.HKEY_CLASSES_ROOT, reg_ext)
                    try:
                        win_type = winreg.QueryValueEx(key, "PerceivedType")[0]
                        winreg.CloseKey(key)
                        if(file_type == ""):
                            file_type = f"{ext} {sel_obj} {win_type}"
                        else:
                            file_type = file_type+"\n" + \
                                f"{ext} {sel_obj} {win_type}"
                        to_log = "win_type = winreg.QueryValueEx(key, \"PerceivedType\")[0]"
                        to_log_var = key, win_type
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                    except FileNotFoundError:
                        if(file_type == ""):
                            file_type = f"{ext} {sel_obj}"
                        else:
                            file_type = file_type+"\n"+f"{ext} {sel_obj}"
                        #fordev_print("reg_notfound")
                        to_log = "reg_key , reg not found"
                        to_log_var = key
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                else:
                    if(file_type == ""):
                        file_type = f"{ext} {sel_obj}"
                    else:
                        file_type = file_type+"\n"+f"{ext} {sel_obj}"
                    to_log = "ext not in reg_ext_list"
                    to_log_var = ext
                    log_out = logger.b_log(to_log, to_log_var, log, log_var)
                    log = log_out[0]
                    log_var = log_out[1]

        str_operation = ""

        numcheck = 0

        # controlla se ci sono tipi di file simili e fa una lista
        for file in file_type.split("\n"):
            sel_obj_comp = file.split(" ")[len(file.split(" "))-1]
            if(len(file.split(" ")) > 3):
                sel_obj_fcomp = file.split(" ")[1], file.split(" ")[2]
            else:
                sel_obj_fcomp = file.split(" ")[1]
            exists = 0
            for tocompare in file_type.split("\n"):
                if(sel_obj_comp in tocompare.split(" ")[len(tocompare.split(" "))-1]):
                    to_log = "sel_obj_comp in tocompare.split(\" \")[len(tocompare.split(\" \"))-1]"
                    to_log_var = sel_obj_comp, tocompare.split(
                        " ")[len(tocompare.split(" "))-1]
                    log_out = logger.b_log(to_log, to_log_var, log, log_var)
                    log = log_out[0]
                    log_var = log_out[1]
                    exists = exists + 1
            if(exists >= 2):
                newexists = 0
                to_log = "exists >= 2"
                to_log_var = exists
                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                log = log_out[0]
                log_var = log_out[1]
                for tocompare in file_type.split("\n"):
                    cleancomp = tocompare.split(" ")[len(tocompare.split(" "))-1]
                    ext = tocompare.split(" ", 1)[0]
                    ext1 = file.split(" ", 1)[0]
                    if(cleancomp == sel_obj_comp):
                        to_log = "cleancomp == sel_obj_comp"
                        to_log_var = cleancomp, sel_obj_comp
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                        #fordev_print(ext, ext1)
                        if(ext != ext1):
                            to_log = "ext != ext1"
                            to_log_var = ext, ext1
                            log_out = logger.b_log(
                                to_log, to_log_var, log, log_var)
                            log = log_out[0]
                            log_var = log_out[1]
                            numcheck = numcheck + 1
                    if(numcheck > 1):
                        numcheck = 0
                        if(len(tocompare) < len(file)):
                            to_log = "numcheck > 1, len(tocompare) < len(file)"
                            to_log_var = len(tocompare), len(file)
                            log_out = logger.b_log(
                                to_log, to_log_var, log, log_var)
                            log = log_out[0]
                            log_var = log_out[1]
                            replacer = tocompare.split(" ", 1)[1]
                            toreplace = file.split(" ", 1)[1]
                            if(str_operation == ""):
                                str_operation = toreplace+"checkstrnreg"+replacer
                            else:
                                str_operation = str_operation + "\n" + toreplace+"checkstrnreg"+replacer
                        elif(len(tocompare) > len(file)):
                            to_log = "len(tocompare) > len(file)"
                            to_log_var = len(tocompare), len(file)
                            log_out = logger.b_log(
                                to_log, to_log_var, log, log_var)
                            log = log_out[0]
                            log_var = log_out[1]
                            toreplace = tocompare.split(" ", 1)[1]
                            replacer = file.split(" ", 1)[1]
                            if(str_operation == ""):
                                str_operation = toreplace+"checkstrnreg"+replacer
                            else:
                                str_operation = str_operation + "\n" + toreplace+"checkstrnreg"+replacer
                        elif(len(tocompare) == len(file)):
                            to_log = "len(tocompare) == len(file)"
                            to_log_var = len(tocompare), len(file)
                            log_out = logger.b_log(
                                to_log, to_log_var, log, log_var)
                            log = log_out[0]
                            log_var = log_out[1]
                            toreplace = tocompare.split(" ", 1)[1]
                            replacer = file.split(" ", 1)[1]
                            if(str_operation == ""):
                                str_operation = toreplace+"checkstrnreg"+replacer
                            else:
                                str_operation = str_operation + "\n" + toreplace+"checkstrnreg"+replacer

            else:
                newexists = 0
                to_log = "exists < 2"
                to_log_var = exists
                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                log = log_out[0]
                log_var = log_out[1]
                for tocompare in file_type.split("\n"):
                    if(len(tocompare.split(" ")) > 3):
                        sel_obj_fcomp2 = tocompare.split(
                            " ")[1], tocompare.split(" ")[2]
                    else:
                        sel_obj_fcomp2 = tocompare.split(
                            " ")[1]
                    if(str(sel_obj_fcomp) in str(sel_obj_fcomp2)):
                        to_log = "str(sel_obj_fcomp) in str(sel_obj_fcomp2)"
                        to_log_var = str(sel_obj_fcomp), str(sel_obj_fcomp2)
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                        newexists = newexists + 1
                if(newexists >= 2):
                    to_log = "newexists >= 2"
                    to_log_var = newexists
                    log_out = logger.b_log(to_log, to_log_var, log, log_var)
                    log = log_out[0]
                    log_var = log_out[1]
                    for tocompare in file_type.split("\n"):
                        ftoremove = str(file).split(" ")[0]
                        ftocompare = str(file).replace(ftoremove, "")
                        storemove = str(tocompare).split(" ")[0]
                        stocompare = str(tocompare).replace(storemove, "")
                        if(ftocompare in stocompare):
                            to_log = "ftocompare in stocompare"
                            to_log_var = ftocompare, stocompare
                            log_out = logger.b_log(
                                to_log, to_log_var, log, log_var)
                            log = log_out[0]
                            log_var = log_out[1]
                            if(len(ftocompare) < len(stocompare)):
                                to_log = "len(ftocompare) < len(stocompare)"
                                to_log_var = len(ftocompare), len(stocompare)
                                log_out = logger.b_log(
                                    to_log, to_log_var, log, log_var)
                                log = log_out[0]
                                log_var = log_out[1]
                                toreplace = stocompare.split(" ", 1)[1]
                                replacer = ftocompare.split(" ", 1)[1]
                                if(str_operation == ""):
                                    str_operation = toreplace+"checkstr"+replacer
                                else:
                                    str_operation = str_operation + "\n" + toreplace+"checkstr"+replacer
                            elif(len(ftocompare) > len(stocompare)):
                                to_log = "len(ftocompare) > len(stocompare)"
                                to_log_var = len(ftocompare), len(stocompare)
                                log_out = logger.b_log(
                                    to_log, to_log_var, log, log_var)
                                log = log_out[0]
                                log_var = log_out[1]
                                replacer = stocompare.split(" ", 1)[1]
                                toreplace = ftocompare.split(" ", 1)[1]
                                if(str_operation == ""):
                                    str_operation = toreplace+"checkstr"+replacer
                                else:
                                    str_operation = str_operation + "\n" + toreplace+"checkstr"+replacer
                            elif(len(ftocompare) == len(stocompare)):
                                to_log = "len(ftocompare) == len(stocompare)"
                                to_log_var = len(ftocompare), len(stocompare)
                                log_out = logger.b_log(
                                    to_log, to_log_var, log, log_var)
                                log = log_out[0]
                                log_var = log_out[1]
                                replacer = stocompare.split(" ", 1)[1]
                                toreplace = ftocompare.split(" ", 1)[1]
                                if(str_operation == ""):
                                    str_operation = toreplace+"checkstr"+replacer
                                else:
                                    str_operation = str_operation + "\n" + toreplace+"checkstr"+replacer

        # spostamento dei valori dalla chiave più lunga a quella più corta ed eliminazione della prima
        proceed = 0

        for replace_grp in str_operation.split("\n"):
            inttoreplace = 0
            intreplacer = 0
            twowords = 0
            reg_word = 0
            if("checkstr" in replace_grp):
                twowords = 1
            elif("checkstrnreg" in replace_grp):
                reg_word = 1
            if(reg_word == 1):
                for tocompare in new_type_list.split("\n"):
                    toreplace = str(replace_grp.split(
                        "checkstr")[0]).rsplit(" ", 1)[0]
                    replacer = str(replace_grp.split(
                        "checkstr")[1]).rsplit(" ", 1)[0]
                    #fordev_("toreplace, replacer", toreplace, replacer)
                    trcheck = 0
                    rcheck = 0
                    if(toreplace == tocompare):
                        to_log = "toreplace == tocompare"
                        to_log_var = toreplace, tocompare
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                        trcheck = trcheck + 1
                        inttoreplace = 1
                    elif(replacer == tocompare):
                        to_log = "replacer == tocompare"
                        to_log_var = replacer, tocompare
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                        rcheck = rcheck + 1
                        intreplacer = 1
                    if((trcheck > 0) or (rcheck > 0)):
                        to_log = "(trcheck > 0) or (rcheck > 0)"
                        to_log_var = trcheck, rcheck
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                        proceed = proceed + 1
                if(proceed > 0):
                    if(inttoreplace == 1):
                        to_log = "groups before"
                        to_log_var = groups
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                        replacer = str(replace_grp.split(
                            "checkstrnreg")[1]).rsplit(" ", 1)[0]
                        toreplace = str(replace_grp.split("checkstrnreg")[0])
                        #fordev_print("toreplace, replacer, inttoreplace == 1",
                        #      toreplace, replacer)
                        for tomove in groups[toreplace]:
                            groups[replacer].append(tomove)
                        groups.pop(toreplace, "")
                        new_type_list = new_type_list.replace(toreplace, replacer)
                        to_log = "proceed > 0, inttoreplace == 1 - groups[replacer] = groups.pop(toreplace), groups after"
                        to_log_var = inttoreplace, replacer, toreplace, groups
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                    elif(intreplacer == 1):
                        to_log = "groups before"
                        to_log_var = groups
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
                        replacer = str(replace_grp.split("checkstrnreg")[1])
                        toreplace = str(replace_grp.split(
                            "checkstrnreg")[0]).rsplit(" ", 1)[0]
                        #fordev_print("toreplace, replacer, intreplacer == 1",
                        #      toreplace, replacer)
                        for tomove in groups[toreplace]:
                            groups[replacer].append(tomove)
                        groups.pop(toreplace, "")
                        new_type_list = new_type_list.replace(toreplace, replacer)
                        to_log = "intreplacer == 1 - groups[replacer] = groups.pop(toreplace), groups after"
                        to_log_var = intreplacer, replacer, toreplace, groups
                        log_out = logger.b_log(to_log, to_log_var, log, log_var)
                        log = log_out[0]
                        log_var = log_out[1]
            elif(twowords == 1):
                to_log = "twowords == 1"
                to_log_var = twowords
                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                log = log_out[0]
                log_var = log_out[1]
                check = 0
                for tocheck in list(groups):
                    if("checkstrnreg" in str(replace_grp)):
                        replacer = str(replace_grp.split(
                            "checkstrnreg")[1]).rsplit(" ", 1)[0]
                        toreplace = str(replace_grp.split(
                            "checkstrnreg")[0]).rsplit(" ", 1)[0]
                    elif("checkstr" in str(replace_grp)):
                        replacer = str(replace_grp.split("checkstr")[1])
                        toreplace = str(replace_grp.split("checkstr")[0])
                    if(toreplace == tocheck):
                        check = 1
                    if(toreplace != replacer):
                        if(check == 1):
                            if(toreplace != ""):
                                to_log = "toreplace != \"\", check == 1"
                                to_log_var = toreplace, check
                                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                                log = log_out[0]
                                log_var = log_out[1]
                                #fordev_print("replacer, toreplace, replace_grp",replacer, "-", toreplace, "-", replace_grp)
                                for tomove in groups[toreplace]:
                                    groups[replacer].append(tomove)
                                #fordev_print("toreplace",toreplace)
                                groups.pop(toreplace, "")
                                new_type_list = new_type_list.replace(toreplace, replacer)
                                replace_grp = replace_grp.replace(toreplace, "")
                        elif(check == 0):
                            if(toreplace != ""):
                                toreplace = toreplace.rsplit(" ", 1)[0]
                                if(toreplace == tocheck):
                                    to_log = "toreplace != \"\", check == 0, toreplace == tocheck"
                                    to_log_var = toreplace, check, tocheck
                                    log_out = logger.b_log(to_log, to_log_var, log, log_var)
                                    log = log_out[0]
                                    log_var = log_out[1]
                                    for tomove in groups[toreplace]:
                                        groups[replacer].append(tomove)
                                    groups.pop(toreplace, "")
                                    new_type_list = new_type_list.replace(
                                        toreplace, replacer)

    # -------------------------------------------------------------------------------------------------------------------------------------------------

    target_dir = direc+conf_dir+"\\"
    target_dir_list = os.listdir(direc+conf_dir)
    tojoin = " "
    new = ""

    # check dei gruppi
    for grp_check in list(groups):
        if(grp_check.endswith(" ")):
            to_log = "grp_check.endswith(\" \")"
            to_log_var = grp_check.endswith(" ")
            log_out = logger.b_log(to_log, to_log_var, log, log_var)
            log = log_out[0]
            log_var = log_out[1]
            #fordev_print(to_log)
            li = grp_check.rsplit(" ", 1)
            groups[new.join(li)] = groups.pop(grp_check)

    for dir_toCreate in groups:
        if(dir_toCreate not in target_dir_list):
            to_log = "dir_toCreate not in target_dir_list"
            to_log_var = dir_toCreate, target_dir_list
            log_out = logger.b_log(to_log, to_log_var, log, log_var)
            log = log_out[0]
            log_var = log_out[1]
            #fordev_print(to_log)
            out = f">>> Cartella creata: {target_dir}{dir_toCreate}<<<\n"
            output = str(output) + str(out)
            forerror = target_dir+dir_toCreate
            try:
                os.mkdir(target_dir+dir_toCreate)
            except Exception as err:
                to_log = str(err)
                log = logger.n_log(to_log, log)
                if(errore == ""):
                    errore = str(err)
                else:
                    errore = errore+"\n" + \
                        str(err)
            num_of_created_dirs = num_of_created_dirs+1

    ignored_files = 0
    moved_files = ""
    group_num = 0
    mv_files_num = 0

    if(backup == True and delbackup == False):
        backupper.backup(datetime, backfolder, new_type_list,
                         new, groups, direc, stat, assoluta, isbfolder)

    elif(backup == True and delbackup == True):
        backupper.delbackup(datetime, backfolder, new_type_list,
                            new, groups, direc, stat, assoluta, isbfolder)

    if(errore == " "):
        errore = ""

    if(optype == "move"):
        to_log = "OPTYPE == MOVE"
        #fordev_print(to_log)
        log = logger.n_log(to_log, log)
        for group_type in new_type_list.split("\n"):
            if(group_type.endswith(" ")):
                to_log = "group_type.endswith(\" \")"
                to_log_var = group_type.endswith(" ")
                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                log = log_out[0]
                log_var = log_out[1]
                #fordev_print(to_log)
                li = group_type.rsplit(" ", 1)
                group_type = new.join(li)
            if(group_type != ""):
                to_log = "group_type != \"\""
                to_log_var = group_type
                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                log = log_out[0]
                log_var = log_out[1]
                #fordev_print(to_log)
                for group_num in range(len(groups[group_type])):
                    selected_file = groups[group_type][group_num]
                    check_final_dir = os.listdir(target_dir+group_type)
                    if(str(selected_file) in os.listdir(direc)):
                        if(str(selected_file).lower() not in str(check_final_dir).lower()):
                            to_log = "str(selected_file).lower() not in str(check_final_dir).lower()"
                            to_log_var = str(selected_file).lower(), str(
                                check_final_dir).lower()
                            log_out = logger.b_log(
                                to_log, to_log_var, log, log_var)
                            log = log_out[0]
                            log_var = log_out[1]
                            #fordev_print(to_log)
                            if("Organizer.py" not in selected_file):
                                moved_files = ""
                                forerror = direc+selected_file, "in", target_dir+group_type
                                try:
                                    shutil.move(direc+selected_file,
                                                target_dir+group_type)
                                    to_log = "MOVE ACTION"
                                    #fordev_print(to_log)
                                    log = logger.n_log(to_log, log)
                                    out = f"{selected_file} spostato in {group_type}\n"
                                    output = str(output) + str(out)
                                    mv_files_num = mv_files_num+1
                                except Exception as err:
                                    to_log = str(err)
                                    log = logger.n_log(to_log, log)
                                    if(errore == ""):
                                        errore = str(err)
                                    else:
                                        errore = errore+"\n" + \
                                            str(err)
                        else:
                            to_log = "ignore file"
                            to_log_var = str(selected_file).lower(), str(
                                check_final_dir).lower()
                            log_out = logger.b_log(
                                to_log, to_log_var, log, log_var)
                            log = log_out[0]
                            log_var = log_out[1]
                            #fordev_print(to_log)
                            ignored_files = ignored_files+1

    elif(optype == "copy"):
        to_log = "OPTYPE == COPY"
        #fordev_print(to_log)
        log = logger.n_log(to_log, log)
        for group_type in new_type_list.split("\n"):
            if(group_type.endswith(" ")):
                to_log = "group_type.endswith(\" \")"
                to_log_var = group_type.endswith(" ")
                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                log = log_out[0]
                log_var = log_out[1]
                #fordev_print(to_log)
                li = group_type.rsplit(" ", 1)
                group_type = new.join(li)
            if(group_type != ""):
                to_log = "group_type != \"\""
                to_log_var = group_type
                log_out = logger.b_log(to_log, to_log_var, log, log_var)
                log = log_out[0]
                log_var = log_out[1]
                #fordev_print(to_log)
                for group_num in range(len(groups[group_type])):
                    selected_file = groups[group_type][group_num]
                    check_final_dir = os.listdir(target_dir+group_type)
                    if(str(selected_file) in os.listdir(direc)):
                        if(str(selected_file).lower() not in str(check_final_dir).lower()):
                            to_log = "str(selected_file).lower() not in str(check_final_dir).lower()"
                            to_log_var = str(selected_file).lower(), str(
                                check_final_dir).lower()
                            log_out = logger.b_log(
                                to_log, to_log_var, log, log_var)
                            log = log_out[0]
                            log_var = log_out[1]
                            #fordev_print(to_log)
                            if("organizer" not in str(selected_file).lower()):
                                #fordev_print("inside if organizer")
                                moved_files = ""
                                forerror = direc+selected_file, "in", target_dir+group_type
                                try:
                                    shutil.copy(direc+selected_file,
                                                target_dir+group_type)
                                    to_log = "COPY ACTION"
                                    #fordev_print(to_log)
                                    log = logger.n_log(to_log, log)
                                    out = f"{selected_file} copiato in {group_type}\n"
                                    output = str(output) + str(out)
                                    mv_files_num = mv_files_num+1
                                except Exception as err:
                                    to_log = str(err)
                                    log = logger.n_log(to_log, log)
                                    #fordev_print(str(err))
                                    if(errore == ""):
                                        errore = str(str(err))
                                    else:
                                        errore = errore+"\n" + \
                                            str(str(err))
                        else:
                            to_log = "ignore file"
                            to_log_var = str(selected_file).lower(), str(
                                check_final_dir).lower()
                            log_out = logger.b_log(
                                to_log, to_log_var, log, log_var)
                            log = log_out[0]
                            log_var = log_out[1]
                            #fordev_print(to_log)
                            ignored_files = ignored_files+1

    if(errore == " "):
        errore = ""

    text_file = open(r"C:\ProgramData\File Organizer\history.txt", "a")
    rawdata = datetime.now()
    datat = rawdata.strftime("%d/%m/%Y %H:%M:%S")
    finaldata = datat.replace("/", "-").replace(":", ".")
    tohistory = f"{output}\n\n/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\ \n\nOPERAZIONE EFFETTUATA ALLE {finaldata}\n\n------------------------------------------------------------------------------------\n\n"
    n = text_file.write(tohistory)
    text_file.close()

    if(optype == "move"):
        if(mv_files_num == 0):
            if(ignored_files > 1 or ignored_files == 0):
                out = f">>> Non è stato spostato nulla ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
                output = str(output) + str(out)
            else:
                out = f">>> Non è stato spostato nulla ({ignored_files} file ignorato, {num_of_created_dirs} cartelle create) <<<"
                output = str(output) + str(out)
        if((ignored_files > 1 or ignored_files == 0) and mv_files_num > 1):
            out = f">>> Sono stati spostati {mv_files_num} file ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
        elif(ignored_files == 1 and mv_files_num > 1):
            out = f">>> Sono stati spostati {mv_files_num} file ({ignored_files} file ignorato, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
        elif(mv_files_num == 1 and (ignored_files > 1 or ignored_files == 0)):
            out = f">>> È stato spostato {mv_files_num} file ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
        logfile = open(r"C:\ProgramData\File Organizer\log.txt", "w")
        n = logfile.write(log+"\nERRORI:\n"+errore)
        logfile.close()
        varlogfile = open(r"C:\ProgramData\File Organizer\logvar.txt", "w")
        n = varlogfile.write(log_var)
        varlogfile.close()
        return output, errore

    elif(optype == "copy"):
        if(mv_files_num == 0):
            #fordev_print("mv_files_num == 0")
            if(ignored_files > 1 or ignored_files == 0):
                out = f">>> Non è stato copiato nulla ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
                output = str(output) + str(out)
            else:
                out = f">>> Non è stato copiato nulla ({ignored_files} file ignorato, {num_of_created_dirs} cartelle create) <<<"
                output = str(output) + str(out)
        if((ignored_files > 1 or ignored_files == 0) and mv_files_num > 1):
            #fordev_print("ignored_files > 1 or ignored_files == 0) and mv_files_num > 1")
            out = f">>> Sono stati copiati {mv_files_num} file ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
            #fordev_print(output)
        elif(ignored_files == 1 and mv_files_num > 1):
            #fordev_print("ignored_files == 1 and mv_files_num > 1")
            out = f">>> Sono stati copiati {mv_files_num} file ({ignored_files} file ignorato, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
        elif(mv_files_num == 1 and (ignored_files > 1 or ignored_files == 0)):
            #fordev_print("#fordev_printmv_files_num == 0)")
            out = f">>> È stato copiato {mv_files_num} file ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
        logfile = open(r"C:\ProgramData\File Organizer\log.txt", "w")
        n = logfile.write(log+"\nERRORI:\n"+errore)
        logfile.close()
        varlogfile = open(r"C:\ProgramData\File Organizer\logvar.txt", "w")
        n = varlogfile.write(log_var)
        varlogfile.close()
        return output, errore


def update(directory, configfile, datafile, new):
    import json
    import os
    output = ""
    is_main_dir_present = 0
    direc = directory
    if("np.txt" in os.listdir(r"C:\ProgramData\File Organizer")):
        with open("C:\ProgramData\File Organizer\config.txt") as f:
            lines = f.readlines()
        with open("C:\ProgramData\File Organizer\\np.txt") as f:
            data = f.readlines()
        datadict = json.loads(data[0])
        configdict = json.loads(lines[0])
        conf_dir = configdict["dir"]
        main_dir = os.listdir(direc)
        main_dir_check = 0
        for file in main_dir:
            if(file == new):
                main_dir_check = main_dir_check+1
            if(main_dir_check == 0):
                if((file == datadict["name"] and file != new) or (file.lower() == str(datadict["name"]).lower() and file.lower() != str(new).lower())):
                    is_main_dir_present = is_main_dir_present+1

        if(is_main_dir_present > 0):
            os.rename(direc+datadict["name"], direc+new)
            output = "Cartella rinominata"
            datadict["name"] = new
            text_file = open(r"C:\ProgramData\File Organizer\np.txt", "w")
            n = text_file.write(json.dumps(datadict))
            text_file.close()
        elif(new in main_dir):
            output = "La cartella ha già questo nome"
        else:
            output = "La cartella non è stata ancora creata"
    return output
