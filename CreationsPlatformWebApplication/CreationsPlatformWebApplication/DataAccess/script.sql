﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240819101121_IntialCreate') THEN
    CREATE TABLE "Users" (
        id uuid NOT NULL,
        login character varying(75) NOT NULL,
        email character varying(100) NOT NULL,
        password_hash text NOT NULL,
        created_date timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL DEFAULT FALSE,
        CONSTRAINT "Users_pkey" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240819101121_IntialCreate') THEN
    CREATE TABLE "Creations" (
        id integer GENERATED BY DEFAULT AS IDENTITY,
        genre character varying[] NOT NULL,
        publication_date timestamp with time zone NOT NULL,
        rating integer NOT NULL DEFAULT 0,
        rating_count integer NOT NULL DEFAULT 0,
        author_id uuid NOT NULL,
        CONSTRAINT "Creations_pkey" PRIMARY KEY (id),
        CONSTRAINT author_id_fk FOREIGN KEY (author_id) REFERENCES "Users" (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240819101121_IntialCreate') THEN
    CREATE TABLE "Comments" (
        id integer GENERATED BY DEFAULT AS IDENTITY,
        user_id uuid NOT NULL,
        creation_id integer NOT NULL,
        content text NOT NULL,
        CONSTRAINT "Comments_pkey" PRIMARY KEY (id),
        CONSTRAINT creation_id_fk FOREIGN KEY (creation_id) REFERENCES "Creations" (id),
        CONSTRAINT user_id_fk FOREIGN KEY (user_id) REFERENCES "Users" (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240819101121_IntialCreate') THEN
    CREATE INDEX "IX_Comments_creation_id" ON "Comments" (creation_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240819101121_IntialCreate') THEN
    CREATE INDEX "IX_Comments_user_id" ON "Comments" (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240819101121_IntialCreate') THEN
    CREATE INDEX "IX_Creations_author_id" ON "Creations" (author_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240819101121_IntialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240819101121_IntialCreate', '8.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240828074247_CreationTitleColumn') THEN
    ALTER TABLE "Users" RENAME COLUMN login TO username;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240828074247_CreationTitleColumn') THEN
    ALTER TABLE "Creations" ADD title text NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240828074247_CreationTitleColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240828074247_CreationTitleColumn', '8.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240828143614_CreationContentColumn') THEN
    ALTER TABLE "Creations" ADD content text NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240828143614_CreationContentColumn') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240828143614_CreationContentColumn', '8.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240829100717_GenresChange') THEN
    ALTER TABLE "Creations" DROP COLUMN genre;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240829100717_GenresChange') THEN
    CREATE TABLE "Genres" (
        id integer GENERATED BY DEFAULT AS IDENTITY,
        name character varying(100) NOT NULL,
        CONSTRAINT "Genres_pkey" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240829100717_GenresChange') THEN
    CREATE TABLE "CreationEntityGenreEntity" (
        "CreationsId" integer NOT NULL,
        "GenresId" integer NOT NULL,
        CONSTRAINT "PK_CreationEntityGenreEntity" PRIMARY KEY ("CreationsId", "GenresId"),
        CONSTRAINT "FK_CreationEntityGenreEntity_Creations_CreationsId" FOREIGN KEY ("CreationsId") REFERENCES "Creations" (id) ON DELETE CASCADE,
        CONSTRAINT "FK_CreationEntityGenreEntity_Genres_GenresId" FOREIGN KEY ("GenresId") REFERENCES "Genres" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240829100717_GenresChange') THEN
    CREATE INDEX "IX_CreationEntityGenreEntity_GenresId" ON "CreationEntityGenreEntity" ("GenresId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240829100717_GenresChange') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240829100717_GenresChange', '8.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171235_CommentTableChange') THEN
    ALTER TABLE "Comments" DROP CONSTRAINT user_id_fk;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171235_CommentTableChange') THEN
    ALTER TABLE "CreationEntityGenreEntity" DROP CONSTRAINT "FK_CreationEntityGenreEntity_Creations_CreationsId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171235_CommentTableChange') THEN
    DROP INDEX "IX_Comments_user_id";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171235_CommentTableChange') THEN
    ALTER TABLE "CreationEntityGenreEntity" RENAME COLUMN "CreationsId" TO "CreationEntityId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171235_CommentTableChange') THEN
    ALTER TABLE "Creations" ADD comment_count integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171235_CommentTableChange') THEN
    ALTER TABLE "Comments" ADD "UserEntityId" uuid;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171235_CommentTableChange') THEN
    CREATE INDEX "IX_Comments_UserEntityId" ON "Comments" ("UserEntityId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171235_CommentTableChange') THEN
    ALTER TABLE "Comments" ADD CONSTRAINT "FK_Comments_Users_UserEntityId" FOREIGN KEY ("UserEntityId") REFERENCES "Users" (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171235_CommentTableChange') THEN
    ALTER TABLE "CreationEntityGenreEntity" ADD CONSTRAINT "FK_CreationEntityGenreEntity_Creations_CreationEntityId" FOREIGN KEY ("CreationEntityId") REFERENCES "Creations" (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171235_CommentTableChange') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240831171235_CommentTableChange', '8.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171706_UserAndCommentLinkChange') THEN
    ALTER TABLE "Comments" DROP CONSTRAINT "FK_Comments_Users_UserEntityId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171706_UserAndCommentLinkChange') THEN
    DROP INDEX "IX_Comments_UserEntityId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171706_UserAndCommentLinkChange') THEN
    ALTER TABLE "Comments" DROP COLUMN "UserEntityId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831171706_UserAndCommentLinkChange') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240831171706_UserAndCommentLinkChange', '8.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831181801_UserAndCommentLinkChange2') THEN
    ALTER TABLE "Comments" DROP CONSTRAINT creation_id_fk;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831181801_UserAndCommentLinkChange2') THEN
    CREATE INDEX "IX_Comments_user_id" ON "Comments" (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831181801_UserAndCommentLinkChange2') THEN
    ALTER TABLE "Comments" ADD CONSTRAINT "FK_Comments_Creations_creation_id" FOREIGN KEY (creation_id) REFERENCES "Creations" (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831181801_UserAndCommentLinkChange2') THEN
    ALTER TABLE "Comments" ADD CONSTRAINT "FK_Comments_Users_user_id" FOREIGN KEY (user_id) REFERENCES "Users" (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831181801_UserAndCommentLinkChange2') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240831181801_UserAndCommentLinkChange2', '8.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831182926_CreationAndCommentLinkChange') THEN
    ALTER TABLE "Comments" ADD publication_date timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240831182926_CreationAndCommentLinkChange') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240831182926_CreationAndCommentLinkChange', '8.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240901060844_AddedRatingColumnToComment') THEN
    ALTER TABLE "Creations" RENAME COLUMN rating TO total_rating;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240901060844_AddedRatingColumnToComment') THEN
    ALTER TABLE "Comments" ADD rating integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240901060844_AddedRatingColumnToComment') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240901060844_AddedRatingColumnToComment', '8.0.8');
    END IF;
END $EF$;
COMMIT;

