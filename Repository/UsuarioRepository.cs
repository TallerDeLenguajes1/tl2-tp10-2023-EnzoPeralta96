namespace tl2_tp10_2023_EnzoPeralta96.Repository.Usuario;
using System.Data.SqlClient;
using System.Data.SQLite;
using tl2_tp10_2023_EnzoPeralta96.Models.Usuario;
public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _cadenaDeConexion;

    public UsuarioRepository(string CadenaDeConexion)
    {
        _cadenaDeConexion = CadenaDeConexion;
    }

    public Usuario UserExists(string nombre, string pass)
    {
        var query = $"SELECT * FROM usuario WHERE nombre_de_usuario = @nombre AND password = @pass AND activo = 1";
        Usuario usuario = null;
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@nombre", nombre));
            command.Parameters.Add(new SQLiteParameter("@pass", pass));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre_de_usuario = reader["nombre_de_usuario"].ToString(),
                        Password = reader["password"].ToString(),
                        Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), reader["rol"].ToString()),
                        Activo = Convert.ToInt32(reader["activo"])
                    };
                }
            }
            conexion.Close();
        }
        //if (usuario == null) throw new Exception("Usuario no encontrado");
        return usuario;
    }

    public bool NameInUse(string nombre)
    {
        var query = $"SELECT COUNT(*) FROM usuario WHERE nombre_de_usuario = @nombre";
        bool nameInUse = false;
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();

            var command = new SQLiteCommand(query, conexion);

            command.Parameters.Add(new SQLiteParameter("@nombre", nombre));

            int count = Convert.ToInt32(command.ExecuteScalar());

            nameInUse = count > 0;

            conexion.Close();
        }
        return nameInUse;
    }

    public bool NameInUseUpdate(string nombre, int idUsuario)
    {
        var query = $"SELECT COUNT(*) FROM usuario WHERE nombre_de_usuario = @nombre AND id <> @idUsuario";
        bool nameInUse = false;
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@nombre", nombre));
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            int count = Convert.ToInt32(command.ExecuteScalar());

            nameInUse = count > 0;

            conexion.Close();
        }
        return nameInUse;
    }

    public bool IsUserValid(int idUsuario)
    {
        var query = $"SELECT COUNT(id) FROM usuario WHERE id = @idUsuario AND id IN (SELECT id FROM usuario)";
        bool userValid = false;
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            int count = Convert.ToInt32(command.ExecuteScalar());

            userValid = count > 0;

            conexion.Close();
        }
        return userValid;
    }

    public void Create(Usuario usuario)
    {
        var query = $"INSERT INTO usuario(nombre_de_usuario,password,rol) VALUES(@nombre,@pass,@rol)";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@nombre", usuario.Nombre_de_usuario));
            command.Parameters.Add(new SQLiteParameter("@pass", usuario.Password));
            var rolString = Enum.GetName(typeof(RolUsuario), usuario.Rol);
            command.Parameters.Add(new SQLiteParameter("@rol", rolString));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public void Update(int idUsuario, Usuario usuario)
    {
        var query = $"UPDATE usuario SET nombre_de_usuario = @nombre, password = @pass WHERE id = @idUsuario";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@nombre", usuario.Nombre_de_usuario));
            command.Parameters.Add(new SQLiteParameter("@pass", usuario.Password));
            /*var rolString = Enum.GetName(typeof(RolUsuario), usuario.Rol);
            command.Parameters.Add(new SQLiteParameter("@rol", rolString));*/
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public Usuario GetUsuarioById(int idUsuario)
    {
        var query = $"SELECT * FROM usuario WHERE id = @idUsuario AND activo = 1";
        bool usuarioEncontrado = false;
        var usuario = new Usuario();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.Nombre_de_usuario = reader["nombre_de_usuario"].ToString();
                    usuario.Password = reader["password"].ToString();
                    usuario.Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), reader["rol"].ToString());
                    usuario.Activo = Convert.ToInt32(reader["activo"]);
                    usuarioEncontrado = true;
                }
            }
            conexion.Close();
        }
        if (!usuarioEncontrado) throw new Exception("Usuario no encontrado");
        return usuario;
    }

    public List<Usuario> GetRestUsers(int idUsuario)
    {
        var query = $"SELECT * FROM usuario WHERE id <> @idUsuario AND activo = 1";
        var usuarios = new List<Usuario>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre_de_usuario = reader["nombre_de_usuario"].ToString(),
                        Password = reader["password"].ToString(),
                        Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), reader["rol"].ToString()),
                        Activo = Convert.ToInt32(reader["activo"])
                    };
                    usuarios.Add(usuario);
                }
            }
            conexion.Close();
        }
        return usuarios;
    }
    public List<Usuario> GetAllUsers()
    {
        var query = $"SELECT * FROM usuario WHERE activo = 1";
        var usuarios = new List<Usuario>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre_de_usuario = reader["nombre_de_usuario"].ToString(),
                        Password = reader["password"].ToString(),
                        Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), reader["rol"].ToString()),
                        Activo = Convert.ToInt32(reader["activo"])
                    };
                    usuarios.Add(usuario);
                }
            }
            conexion.Close();
        }
        return usuarios;
    }


    public void Delete(int idUsuario)
    {
        deleteTareasByTablero(idUsuario);
        deleteTableroByUsuario(idUsuario);
        deleteUser(idUsuario);
    }

    private void deleteUser(int idUsuario)
    {
        var query = $"UPDATE usuario SET activo = 0 WHERE id = @idUsuario";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }



    private void deleteTableroByUsuario(int idUsuario)
    {
        var query = $"UPDATE tablero SET activo = 0 WHERE id_usuario_propietario =  @idUsuario";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    private void deleteTareasByTablero(int idUsuario)
    {
        var query = $"UPDATE tarea SET activo = 0 WHERE id_tablero IN (SELECT id FROM tablero WHERE id_usuario_propietario =  @idUsuario)";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

}






