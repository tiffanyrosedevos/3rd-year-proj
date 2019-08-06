using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blush.Models;
using System.Web.Security;
using System.Configuration;
using System.Data.SqlClient;

namespace Blush
{
    public class MyRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            String CS = ConfigurationManager.ConnectionStrings["DataModel"].ConnectionString;
            SqlConnection connection = new SqlConnection(CS);
            string cmd = "Select [Type] from [User] where [Username]='" + username + "'";
            connection.Open();
            SqlCommand command = new SqlCommand(cmd, connection);
            SqlDataReader reader = command.ExecuteReader();
            string[] roles = new string[1];
            while(reader.Read())
                roles[0] = reader.GetString(0);
            connection.Close();
            reader.Close();
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            String CS = ConfigurationManager.ConnectionStrings["DataModel"].ConnectionString;
            SqlConnection connection = new SqlConnection(CS);
            string cmd = "Select [Type] from [User] where [Username]='" + username + "'";
            connection.Open();
            SqlCommand command = new SqlCommand(cmd, connection);
            SqlDataReader reader = command.ExecuteReader();
            string role = "";
            while (reader.Read())
                role = reader.GetString(0);
            connection.Close();
            reader.Close();
            return (role.Equals(roleName));
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}