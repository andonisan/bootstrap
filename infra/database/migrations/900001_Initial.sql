
CREATE SCHEMA  todos;

CREATE TABLE todos.todos (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "Completed" boolean NOT NULL,
    CONSTRAINT "PK_todos" PRIMARY KEY ("Id")
);

