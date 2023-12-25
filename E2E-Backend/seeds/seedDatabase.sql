\c "E2EDatabase";

CREATE TABLE users (
    ID SERIAL PRIMARY KEY,
    Email varchar(64) NOT NULL,
    Username varchar(64) NOT NULL,
    PublicKey varchar(256) NOT NULL,
    PasswordHash bytea NOT NULL,
    PasswordSalt bytea NOT NULL
)