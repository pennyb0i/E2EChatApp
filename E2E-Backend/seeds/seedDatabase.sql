\c "E2EDatabase";

CREATE TABLE users (
    ID SERIAL PRIMARY KEY,
    Email varchar(64) NOT NULL,
    PasswordHash bytea NOT NULL,
    PasswordSalt bytea NOT NULL
)