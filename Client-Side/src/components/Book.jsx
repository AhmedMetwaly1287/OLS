import React, { useState, useEffect } from "react";
import axios from "axios";
import "./assets/css/Book.css";
import { Link, useParams } from "react-router-dom";
import Logout from "./Logout";
import { jwtDecode } from "jwt-decode";

const Book = () => {
  const { BookID } = useParams();
  const [book, setBook] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [borrowMessage, setBorrowMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  useEffect(() => {
    fetchBook();
  }, []);

  const fetchBook = async () => {
    setIsLoading(true);
    try {
      const token = sessionStorage.getItem("token");
      const response = await axios.get(
        `http://localhost:5289/api/Book/GetBook/${BookID}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      setBook(response.data);
      setError(null);
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  const handleBorrow = async () => {
    try {
      const token = sessionStorage.getItem("token");
      const decodedToken = jwtDecode(token);
      const UserID = decodedToken.UserID;
      const borrowResponse = await axios.post(
        `http://localhost:5289/api/BorrowedBook/SubmitRequest`,
        { UserID, BookID },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      setSuccessMessage(
        "Request Submitted Successfully, Please await the Librarian's decision"
      );
    } catch (error) {
      setBorrowMessage("You have exceeded the allowed limit of requests");
    }
  };

  return (
    <div className="book-container">
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
      {successMessage && (
        <div className="book-success-message-container">
          <p className="success-message">{successMessage}</p>
        </div>
      )}
      {borrowMessage && (
        <div className="error-message-container">
          <p className="error-message">{borrowMessage}</p>
        </div>
      )}
      {isLoading ? (
        <p>Loading...</p>
      ) : error ? (
        <p className="error-message">{error}</p>
      ) : book ? (
        <div className="book-details">
          <h2 className="book-title">{book.title}</h2>
          <p className="book-info">Author: {book.author}</p>
          <p className="book-info">ISBN: {book.isbn}</p>
          <p className="book-info">Rack Number: {book.rackNumber}</p>
          <p className="book-info">
            Is Borrowed: {book.isBorrowed ? "Yes" : "No"}
          </p>
          <button
            className={`borrow-button ${book.isBorrowed ? "disabled" : ""}`}
            onClick={handleBorrow}
            disabled={book.isBorrowed}
          >
            Borrow
          </button>
        </div>
      ) : (
        <p>No book found with ID {BookID}</p>
      )}
    </div>
  );
};

export default Book;
