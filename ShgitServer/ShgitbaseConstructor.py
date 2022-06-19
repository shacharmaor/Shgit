import mysql.connector

basename = 'shgitbase'
dbpass = 'ApplePie135246'
dbuser = 'root'

try:
  mydb = mysql.connector.connect(
    host="localhost",
    user=dbuser,
    password=dbpass,
    database = basename
  )
  mycursor = mydb.cursor()
except:
  mydb = mysql.connector.connect(
    host="localhost",
    user=dbuser,
    password=dbpass,
  )
  mycursor = mydb.cursor()
  mycursor.execute(f"CREATE DATABASE {basename}")
  mydb = mysql.connector.connect(
    host="localhost",
    user=dbuser,
    password=dbpass,
    database = basename
  )
  mycursor = mydb.cursor()

tables = []
mycursor.execute("SHOW TABLES")
for table in mycursor:
  tables.append(table)
  print(table)

if 'users' not in tables:
  mycursor.execute("CREATE TABLE users (user VARCHAR(255), pass VARCHAR(255), PRIMARY KEY (user))")

if 'graphs' not in tables:
  mycursor.execute("CREATE TABLE graphs (graphname VARCHAR(255), user VARCHAR(255), PRIMARY KEY (graphname, user), FOREIGN KEY (user) REFERENCES users(user), perms INT)")

mydb.commit()
mydb.close()

