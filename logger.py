def b_log(to_log, to_log_var, log, log_var):
    if(log == ""):
        log = "0"+" "+to_log
    else:
        log_len = len(log.split("\n"))
        log = log+"\n"+str(log_len)+" "+to_log
    if(log_var == ""):
        log_var = "0"+" "+str(to_log_var)+"\n"
    else:
        log_var_len = len(log.split("\n"))-1
        log_var = log_var+"\n"+str(log_var_len)+" "+str(to_log_var)
    return log, log_var

def n_log(to_log, log):
    if(log == ""):
        log = "0"+" "+to_log
    else:
        log_len = len(log.split("\n"))
        log = log+"\n"+str(log_len)+" "+to_log
    return log