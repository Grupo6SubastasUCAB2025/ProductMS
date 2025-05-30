#!/bin/bash
set -ex # 'e' para salir en error, 'x' para imprimir comandos

# No necesitamos pg_isready aquí. El docker-entrypoint.sh ya garantiza que PG esté listo.
echo "El servidor PostgreSQL está activo y listo para la inicialización."

# Verificar si la base de datos ya existe
# Usamos 'psql -lqt' para listar bases de datos y grep para buscar el nombre.
# La salida de psql debe ser limpia, por eso '-tAc' para obtener solo la tupla y el conteo.
DB_EXISTS=$(psql -U "$POSTGRES_USER" -lqt | cut -d \| -f 1 | grep -w "$POSTGRES_DB" | wc -l)

if [ "$DB_EXISTS" -eq 0 ]; then
    echo "La base de datos '$POSTGRES_DB' no existe. Creándola..."
    createdb -U "$POSTGRES_USER" "$POSTGRES_DB"
    echo "Base de datos '$POSTGRES_DB' creada."
else
    echo "La base de datos '$POSTGRES_DB' ya existe."
fi

# Verificar si la base de datos ya contiene datos (por ejemplo, verificando si hay tablas)
# Intentamos conectar a la base de datos y listar algunas tablas del esquema público.
# Si no hay tablas, asumimos que está vacía y necesita restauración.
TABLE_COUNT=$(psql -U "$POSTGRES_USER" -d "$POSTGRES_DB" -tAc "SELECT count(*) FROM pg_tables WHERE schemaname = 'public';" | tr -d '[:space:]')

if [ "$TABLE_COUNT" -eq 0 ]; then
    echo "La base de datos '$POSTGRES_DB' está vacía o no contiene tablas. Procediendo con la restauración..."
    # Ejecuta pg_restore en la base de datos recién creada o vacía
    pg_restore -U "$POSTGRES_USER" --role="$POSTGRES_USER" --no-owner --clean --if-exists -d "$POSTGRES_DB" "/docker-entrypoint-initdb.d/productos_db.backup"
    echo "Restauración de la base de datos '$POSTGRES_DB' completada."
else
    echo "La base de datos '$POSTGRES_DB' ya contiene datos. No se realizará la restauración."
fi

echo "Script de inicialización de PostgreSQL finalizado."