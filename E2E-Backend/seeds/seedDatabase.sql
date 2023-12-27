\c "E2EDatabase";

CREATE TABLE users (
    ID SERIAL PRIMARY KEY,
    Email varchar(64) NOT NULL,
    Username varchar(64) NOT NULL,
    PublicKey varchar(256) NOT NULL,
    PasswordHash bytea NOT NULL,
    PasswordSalt bytea NOT NULL
)

CREATE TABLE messages (
    ID SERIAL PRIMARY KEY,
    SenderId varchar(64) NOT NULL,
    ReceiverId varchar(64) NOT NULL,
    Content Text NOT NULL,
    Timestamp TIMESTAMP,
    FOREIGN KEY (SenderId) REFERENCES users(ID),
    FOREIGN KEY (ReceiverId) REFERENCES users(ID)
);