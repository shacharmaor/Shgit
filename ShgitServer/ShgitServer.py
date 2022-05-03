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
        dbcursor.execute("SELECT user, pass FROM clients")
        user_data = dbcursor.fetchall()
        if (login[0], login[1]) not in user_data:
            logged = False
    elif login[2] == "s":
        sql = "INSERT INTO clients (user, pass) VALUES (%s, %s)"
        val = (login[0], login[1])
        dbcursor.execute(sql, val)

        dbconn.commit()
    return logged

def handle_client(conn, dbconn):
    while not log_in(conn, dbconn):
        conn.send("username or password incorrect".encode())
    conn.send("logged in".encode())

    input = ""
    while(input!="quit"):
        print("loop started")
        input = conn.recv(BUFFER).decode()
        if input == "upload":
            file = conn.recv(BUFFER).decode()
            file_path = "C:\\ShgitServer\\Files\\" + file
            f = open(file_path, "w")
            f.close()
            print("file created")
            with open(file_path, "wb") as f:
                while True:
                    bytes_read = conn.recv(BUFFER)
                    if not bytes_read:
                        break
                    f.write(bytes_read)

    
def main():
    dbconn = mysql.connector.connect(
      host="localhost",
      user="root",
      password="ApplePie135246",
      database="shgitbase"
    )
    
    
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((HOST, PORT))
        s.listen()
        print("listening at " + HOST + " on port " + str(PORT))
        while(True):
            conn, addr = s.accept()
            client = threading.Thread(target=handle_client, args=(conn, dbconn))
            client.start()


if __name__ == "__main__":
    main()