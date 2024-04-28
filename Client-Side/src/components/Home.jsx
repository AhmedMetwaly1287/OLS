import React, { useState, useEffect } from "react";
import axios from "axios";
import "./assets/css/Home.css";
import { Link } from "react-router-dom";
import Logout from "./Logout";

const Home = () => {
  const [books, setBooks] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [isbnFilter, setIsbnFilter] = useState("");
  const [rackNumberFilter, setRackNumberFilter] = useState("");
  const [titleFilter, setTitleFilter] = useState("");
  const [authorFilter, setAuthorFilter] = useState("");

  useEffect(() => {
    fetchBooks();
  }, []);

  const fetchBooks = async () => {
    setIsLoading(true);
    try {
      const token = sessionStorage.getItem("token");
      let url = "http://localhost:5289/api/Book";
      if (isbnFilter) {
        const isbnResponse = await axios.get(
          `${url}/FilterByISBN/${isbnFilter}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );
        setBooks(isbnResponse.data);
      } else if (rackNumberFilter) {
        const rackNumberResponse = await axios.get(
          `${url}/FilterByRN/${rackNumberFilter}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );
        setBooks(rackNumberResponse.data);
      } else if (authorFilter) {
        const authorResponse = await axios.get(
          `${url}/FilterByAuthor/${authorFilter}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );
        setBooks(authorResponse.data);
      } else if (titleFilter) {
        const titleResponse = await axios.get(
          `${url}/FilterByTitle/${titleFilter}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );
        setBooks(titleResponse.data);
      } else {
        const response = await axios.get(`${url}/GetAll`, {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
        setBooks(response.data);
      }
      setError(null);
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  const handleIsbnFilterChange = (event) => {
    setIsbnFilter(event.target.value);
  };

  const handleRackNumberFilterChange = (event) => {
    setRackNumberFilter(event.target.value);
  };

  const handleTitleFilterChange = (event) => {
    setTitleFilter(event.target.value);
  };

  const handleAuthorFilterChange = (event) => {
    setAuthorFilter(event.target.value);
  };

  const handleFilterByIsbn = (event) => {
    if (event.key === "Enter") {
      fetchBooks();
    }
  };

  const handleFilterByRackNumber = (event) => {
    if (event.key === "Enter") {
      fetchBooks();
    }
  };

  const handleFilterByTitle = (event) => {
    if (event.key === "Enter") {
      fetchBooks();
    }
  };

  const handleFilterByAuthor = (event) => {
    if (event.key === "Enter") {
      fetchBooks();
    }
  };

  return (
    <div className="home-container">
      <div className="header">
        <h2 className="logo">OLS</h2>

        <div className="options">
          <div className="option">
            <Link to="/Home">Home</Link>
          </div>
          <div className="option">
            <Link to="/Home/UserRequests">Borrow Requests</Link>
          </div>
          <div className="option">
            <Link to="/Home/Archive">Archive</Link>
          </div>
          <div className="option" onClick={Logout}>
            Logout
          </div>
        </div>
      </div>
      <div className="search-isbn">
        <input
          type="text"
          placeholder="Search by ISBN"
          value={isbnFilter}
          onChange={handleIsbnFilterChange}
          onKeyPress={handleFilterByIsbn}
        />
      </div>
      <div className="search-racknum">
        <input
          type="text"
          placeholder="Search by Rack Number"
          value={rackNumberFilter}
          onChange={handleRackNumberFilterChange}
          onKeyPress={handleFilterByRackNumber}
        />
      </div>
      <div className="search-title">
        <input
          type="text"
          placeholder="Search by Title"
          value={titleFilter}
          onChange={handleTitleFilterChange}
          onKeyPress={handleFilterByTitle}
        />
      </div>
      <div className="search-author">
        <input
          type="text"
          placeholder="Search by Author"
          value={authorFilter}
          onChange={handleAuthorFilterChange}
          onKeyPress={handleFilterByAuthor}
        />
      </div>
      <h3 className="page-title">Available Books</h3>
      <div className="content">
        {isLoading ? (
          <p>Loading...</p>
        ) : error ? (
          <p className="error-message">{error}</p>
        ) : books.length === 0 ? (
          <p className="error-message">No Books Available</p>
        ) : (
          <div className="book-cards">
            {books.map((book) => (
              <div className="book-card" key={book.id}>
                <h3 className="book-title">{book.title}</h3>
                <p className="book-info">Author: {book.author}</p>
                <p className="book-info">ISBN: {book.isbn}</p>
                <p className="book-info">Rack Number: {book.rackNumber}</p>
                <Link to={`Book/${book.id}`} className="view-button">
                  View
                </Link>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default Home;
