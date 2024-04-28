import React, { useState, useEffect } from "react";
import axios from "axios";
import "./assets/css/UpdateBook.css";
import Logout from "./Logout";
import { useParams } from "react-router-dom";
import { Link } from "react-router-dom";

const UpdateBook = () => {
  const { bookId } = useParams(); // Assuming you'll pass bookId as a parameter in the URL
  const [book, setBook] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [successMessage, setSuccessMessage] = useState("");
  const [updatedISBN, setUpdatedISBN] = useState("");
  const [updatedRackNumber, setUpdatedRackNumber] = useState("");
  const [updatedAuthor, setUpdatedAuthor] = useState("");
  const [updatedTitle, setUpdatedTitle] = useState("");

  useEffect(() => {
    fetchBook();
  }, []);

  const fetchBook = async () => {
    setIsLoading(true);
    try {
      const token = sessionStorage.getItem("token");
      const response = await axios.get(
        `http://localhost:5289/api/Book/GetBook/${bookId}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      setBook(response.data);
      setError(null); // Reset error state on successful fetch
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  const handleUpdate = async () => {
    // Check if any field is filled
    if (!updatedISBN && !updatedRackNumber && !updatedTitle && !updatedAuthor) {
      setError("Please fill in atleast one field.");
      return;
    }

    try {
      const token = sessionStorage.getItem("token");
      // Check if the ISBN exists in another book
      if (updatedISBN) {
        const isbnResponse = await axios.get(
          `http://localhost:5289/api/Book/CheckISBN/${updatedISBN}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );
        if (isbnResponse.data) {
          setError("ISBN already exists in another book.");
          return;
        }
      }

      // Prepare data for update
      const updateData = {};
      if (updatedISBN) updateData.isbn = updatedISBN;
      else updateData.isbn = book.isbn;
      if (updatedRackNumber) updateData.rackNumber = updatedRackNumber;
      else updateData.rackNumber = book.rackNumber;
      if (updatedTitle) updateData.title = updatedTitle;
      else updateData.title = book.title;
      if (updatedAuthor) updateData.author = updatedAuthor;
      else updateData.author = book.author;

      // Proceed with updating the book
      setIsLoading(true);
      const response = await axios.patch(
        `http://localhost:5289/api/Book/UpdateBook/${bookId}`,
        updateData,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      setSuccessMessage("Book updated successfully");
      // Refresh the page to retrieve updated book data
      fetchBook();
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="update-book-container">
      <div className="header">
        <h2>OLS</h2>
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
      {isLoading && <p>Loading...</p>}
      {error && <div className="error-message">{error}</div>}
      {successMessage && (
        <div className="success-message">{successMessage}</div>
      )}
      {book && (
        <div className="book-details">
          <p>Current ISBN: {book.isbn}</p>
          <input
            type="text"
            className="update-inputs"
            placeholder="New ISBN"
            value={updatedISBN}
            onChange={(e) => setUpdatedISBN(e.target.value)}
          />
          <p>Current Rack Number: {book.rackNumber}</p>
          <input
            type="text"
            className="update-inputs"
            placeholder="New Rack Number"
            value={updatedRackNumber}
            onChange={(e) => setUpdatedRackNumber(e.target.value)}
          />
          <p>Current Title: {book.title}</p>
          <input
            type="text"
            className="update-inputs"
            placeholder="New Title"
            value={updatedTitle}
            onChange={(e) => setUpdatedTitle(e.target.value)}
          />
          <p>Current Author: {book.author}</p>
          <input
            type="text"
            className="update-inputs"
            placeholder="New Author"
            value={updatedAuthor}
            onChange={(e) => setUpdatedAuthor(e.target.value)}
          />
          <br />
          <button onClick={handleUpdate}>Update</button>
        </div>
      )}
    </div>
  );
};

export default UpdateBook;
