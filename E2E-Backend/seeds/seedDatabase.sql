\c "E2EDatabase";

CREATE TABLE users (
    ID SERIAL PRIMARY KEY,
    Email varchar(64) NOT NULL,
    Username varchar(64) NOT NULL,
    Public_Key varchar(256) NOT NULL,
    Password_Hash bytea NOT NULL,
    Password_Salt bytea NOT NULL
);

CREATE TABLE friendships (
    Sender_Id int NOT NULL 
        CONSTRAINT FK_friendships_Sender REFERENCES users, 
    Receiver_Id int NOT NULL 
        CONSTRAINT FK_friendships_Receiver REFERENCES users,
    Is_Pending bool not NULL DEFAULT TRUE,
    CONSTRAINT PK_friendships PRIMARY KEY (Sender_Id,Receiver_Id),
    CONSTRAINT CHK_friendships
        check (Sender_Id <> Receiver_Id)
);

create unique index UX_friendships on friendships(greatest(Sender_Id,Receiver_Id), least(Sender_Id,Receiver_Id));

    PublicKey varchar(256) NOT NULL,
    PasswordHash bytea NOT NULL,
    PasswordSalt bytea NOT NULL
);

CREATE TABLE messages (
    ID SERIAL PRIMARY KEY,
    Sender_Id integer NOT NULL,
    Receiver_Id integer NOT NULL,
    Content Text NOT NULL,
    Timestamp TIMESTAMP NOT NULL,
    FOREIGN KEY (Sender_Id) REFERENCES users(ID),
    FOREIGN KEY (Receiver_Id) REFERENCES users(ID)
);