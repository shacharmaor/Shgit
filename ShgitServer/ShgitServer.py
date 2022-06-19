# echo-server.py
import mysql.connector
import socket
import threading
import os


HOST = "127.0.0.1"
PORT = 11111
BUFFER = 1024

def log_in(conn, dbconn):
    logged = True
    dbcursor = dbconn.cursor()
    login_data = conn.recv(BUFFER).decode()
    login = login_data.split(' ')
    
    if login[2] == "l":
        dbcursor.execute("SELECT user, pass FROM users")
        user_data = dbcursor.fetchall()
        if (login[0], login[1]) not in user_data:
            logged = False
    elif login[2] == "s":
        sql = "INSERT INTO users (user, pass) VALUES (%s, %s)"
        val = (login[0], login[1])
        dbcursor.execute(sql, val)

        dbconn.commit()
    return logged


def insert_file(conn, dbconn): #insert file into appropriate graph
    dbcursor = dbconn.cursor()
    file = conn.recv(BUFFER).decode().strip('\x00') #remove embedded null characters from the shitty .NET sockets
    file_path = fR'C:\Users\shach\source\repos\Shgit\ShgitServer\Files\{file}' #place to store new commit on server
    print(file_path)
    os.makedirs(os.path.dirname(file_path), exist_ok=True)
    file_size = int(conn.recv(BUFFER).decode().strip('\x00'))
    with open(file_path, 'wb') as f: 
        bytes_read = conn.recv(file_size)
        f.write(bytes_read)
    


def handle_client(conn, dbconn): #handle client input
    while not log_in(conn, dbconn):
        conn.send("username or password incorrect".encode()) #force login
    conn.send("logged in".encode())

    input = ""
    while input!="quit":
        input = conn.recv(BUFFER).decode()
        print(input)
        input = input.split(' ')
        if input[0] == "upload":
            insert_file(conn, dbconn)


def main():
    dbconn = mysql.connector.connect(
      host="localhost",
      user="root",
      password="ApplePie135246",
      database="shgitbase"
    ) #connect to user database
    
    
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((HOST, PORT))
        s.listen()
        print("listening at " + HOST + " on port " + str(PORT))
        while(True):
            conn, addr = s.accept()
            client = threading.Thread(target=handle_client, args=(conn, dbconn)) #start working with new client
            client.start()


if __name__ == "__main__":
    main()