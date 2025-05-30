-- Create user if not exists
DO
$do$
BEGIN
   IF NOT EXISTS (
      SELECT FROM pg_catalog.pg_roles WHERE rolname = 'dockeruser'
   ) THEN
      CREATE USER dockeruser WITH PASSWORD '0000' CREATEDB;
   END IF;
END
$do$;

-- Create database if not exists
DO
$do$
BEGIN
   IF NOT EXISTS (SELECT FROM pg_database WHERE datname = 'productos_db') THEN
      CREATE DATABASE productos_db OWNER dockeruser;
   END IF;
END
$do$;

-- Connect to the database
\c productos_db postgres;

-- Grant privileges
ALTER DATABASE productos_db OWNER TO dockeruser;
GRANT ALL PRIVILEGES ON DATABASE productos_db TO dockeruser;

-- Create schema objects
CREATE TABLE IF NOT EXISTS productos (
    id SERIAL PRIMARY KEY,
    usuario_id INT NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    categoria VARCHAR(50),
    precio_base DECIMAL(12,2) NOT NULL,
    estado VARCHAR(20) DEFAULT 'disponible',
    imagenes TEXT,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

ALTER TABLE productos OWNER TO dockeruser;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO dockeruser;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO dockeruser;
GRANT ALL PRIVILEGES ON SCHEMA public TO dockeruser;
