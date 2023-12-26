\c "E2EDatabase";

CREATE TABLE users (
    ID SERIAL PRIMARY KEY,
    Email varchar(64) NOT NULL,
    Username varchar(64) NOT NULL,
    Public_Key varchar(256) NOT NULL,
    Password_Hash bytea NOT NULL,
    Password_Salt bytea NOT NULL
)

CREATE TABLE friendships (
    Sender_Id int NOT NULL 
        CONSTRAINT FK_friendships_Sender REFERENCES users, 
    Receiver_Id int NOT NULL 
        CONSTRAINT FK_friendships_Receiver REFERENCES users,
    Is_Pending bit not NULL DEFAULT 1,
    CONSTRAINT PK_friendships PRIMARY KEY,
    CONSTRAINT CHK_friendships
        check (friendships.sender_id <> friendships.receiver_id);
)

create unique index UX_friendships on friendships(greatest(Sender_Id,Receiver_Id), least(Sender_Id,Receiver_Id));
