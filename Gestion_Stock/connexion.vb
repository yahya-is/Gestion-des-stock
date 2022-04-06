Imports System.Data.SqlClient

Module connexion
    Public cn As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)

End Module
