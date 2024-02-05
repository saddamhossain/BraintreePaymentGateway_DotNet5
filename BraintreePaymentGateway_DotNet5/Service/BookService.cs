﻿using BraintreePaymentGateway_DotNet5.Context;
using BraintreePaymentGateway_DotNet5.Interface;
using BraintreePaymentGateway_DotNet5.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BraintreePaymentGateway_DotNet5.Service
{
    public class BookService : IBook
    {
        private readonly DatabaseContext _context;
        public BookService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAll()
        {
            var result = _context.Books.ToList();
            return result;
        }

        public Book GetById(Guid id)
        {
            var result = _context.Books.Where(s => s.Id == id).FirstOrDefault();
            return result;
        }
    }
}
