﻿using AuTinder.Models;
using System;
using System.Collections.Generic;
using System.Data;

public class UserRepo
{
    public static void InsertUser(bool driver, string email, string name, string surname, string phone, string address, int searchDistance, int adCount, int orderCount)
    {
        string query = @"
            INSERT INTO user (driver, email, name, surname, phone, address, search_distance, OrderCount, AdCount )
            VALUES (?driver, ?email, ?name, ?surname, ?phone, ?address, ?search_distance, ?order_count, ?ad_count);
            SELECT LAST_INSERT_ID();";

        Sql.Insert(query, args =>
        {
            args.Add("?driver", driver);
            args.Add("?email", email);
            args.Add("?name", name);
            args.Add("?surname", surname);
            args.Add("?phone", phone);
            args.Add("?address", address);
            args.Add("?search_distance", searchDistance);
            args.Add("?ad_count", adCount);
            args.Add("?order_count", orderCount);
        });
    }

    public static void UpdateUser(int id, bool driver, string email, string name, string surname, string phone, string address, int searchDistance, int adCount, int orderCount)
    {
        string query = @"
            UPDATE user 
            SET driver = ?driver, 
                email = ?email, 
                name = ?name, 
                surname = ?surname, 
                phone = ?phone, 
                address = ?address, 
                search_distance = ?search_distance, 
                AdCount = ?ad_count, 
                OrderCount = ?order_count
            WHERE id = ?id;";

        Sql.Update(query, args =>
        {
            args.Add("?id", id);
            args.Add("?driver", driver);
            args.Add("?email", email);
            args.Add("?name", name);
            args.Add("?surname", surname);
            args.Add("?phone", phone);
            args.Add("?address", address);
            args.Add("?search_distance", searchDistance);
            args.Add("?ad_count", adCount);
            args.Add("?order_count", orderCount);
        });
    }

    public static void DeleteUser(int id)
    {
        string query = @"DELETE FROM user WHERE id = ?id;";

        Sql.Delete(query, args =>
        {
            args.Add("?id", id);
        });
    }

    public static User GetUserById(int id)
    {
        string query = @"
            SELECT id, driver, email, name, surname, phone, Address, search_distance, AdCount, OrderCount
            FROM user
            WHERE id = ?id;";

        var rows = Sql.Query(query, args =>
        {
            args.Add("?id", id);
        });

        User user = Sql.MapOne<User>(rows, (extractor, item) =>
        {
            item.Id = extractor.From<int>("id");
            item.Driver = extractor.From<bool>("driver");
            item.Email = extractor.From<string>("email");
            item.Name = extractor.From<string>("name");
            item.Surname = extractor.From<string>("surname");
            item.Phone = extractor.From<string>("phone");
            item.Address = extractor.From<string>("Address");
            item.SearchDistance = extractor.From<int>("search_distance");
            item.AdCount = extractor.From<int>("AdCount");
            item.OrderCount = extractor.From<int>("OrderCount");
        });

        return user;
    }

    public static List<User> GetAllUsers()
    {
        string query = @"
            SELECT id, driver, email, name, surname, phone, Address, search_distance, AdCount, OrderCount
            FROM user;";

        var rows = Sql.Query(query);

        var users = Sql.MapAll<User>(rows, (extractor, item) =>
        {
            item.Id = extractor.From<int>("id");
            item.Driver = extractor.From<bool>("driver");
            item.Email = extractor.From<string>("email");
            item.Name = extractor.From<string>("name");
            item.Surname = extractor.From<string>("surname");
            item.Phone = extractor.From<string>("phone");
            item.Address = extractor.From<string>("Address");
            item.SearchDistance = extractor.From<int>("search_distance");
            item.AdCount = extractor.From<int>("AdCount");
            item.OrderCount = extractor.From<int>("OrderCount");
        });

        return users;
    }
}
