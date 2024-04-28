import React, { useState } from "react";
import axios from "axios";
import "./assets/css/AddBook.css";
import Logout from "./Logout";
import { Link } from "react-router-dom";

const AddBook = () => {
  const [isbn, setIsbn] = useState("");
  const [rackNumber, setRackNumber] = useState("");
  const [title, setTitle] = useState("");
  const [author, setAuthor] = useState("");
  const [error, setError] = useState("");

  const handleAddBook = async () => {
    if (!isbn || !rackNumber || !title || !author) {
      setError("Please fill in all fields.");
      return;
    }
    try {
      const token = sessionStorage.getItem("token");
      await axios.post(
        "http://localhost:5289/api/Book/AddBook",
        {
          isbn,
          rackNumber,
          title,
          author,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      // Redirect to books page on success
      document.location.href = "/Books";
    } catch (error) {
      setError(
        "Error while adding book (ISBN already exists / Server-side Error)"
      );
    }
  };

  return (
    <div className="admin-dashboard-container">
      <div className="admin-dashboard-header">
        <div className="logo">OLS</div>
        <div className="options">
          <div className="option">
            <Link to="/Users">Users</Link>
          </div>
          <div className="option">
            <Link to="/Books">Books</Link>
          </div>
          <div className="option">
            <Link to="/Admin/UserRequests">Borrow Requests</Link>
          </div>
          <div className="option">
            <Link to="/Admin/Archive">Archive</Link>
          </div>
          <div className="option" onClick={Logout}>
            Logout
          </div>
        </div>
      </div>

      <div className="admin-dashboard-content">
        <h2 className="pageheader">Add a Book</h2>
        <div className="form-container">
          <div className="inputs">
            <input
              type="text"
              placeholder="ISBN"
              value={isbn}
              onChange={(e) => setIsbn(e.target.value)}
            />
            <input
              type="text"
              placeholder="Rack Number"
              value={rackNumber}
              onChange={(e) => setRackNumber(e.target.value)}
            />
            <input
              type="text"
              placeholder="Title"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
            />
            <input
              type="text"
              placeholder="Author"
              value={author}
              onChange={(e) => setAuthor(e.target.value)}
            />
          </div>
          <div className="add-btn-container">
            <button className="add-btn" onClick={handleAddBook}>
              Add
            </button>
          </div>

          {error && (
            <div className="error-message-container">
              <p className="error-message">{error}</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default AddBook;
