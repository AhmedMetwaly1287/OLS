import React, { useState, useEffect } from "react";
import axios from "axios";
import "./assets/css/Books.css";
import jsPDF from "jspdf";
import Logout from "./Logout";
import { Link } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

const Books = () => {
  const [books, setBooks] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [successMessage, setSuccessMessage] = useState("");
  const [isbnFilter, setIsbnFilter] = useState("");
  const [rackNumberFilter, setRackNumberFilter] = useState("");

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
      } else {
        const response = await axios.get(`${url}/GetAll`, {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
        setBooks(response.data);
      }
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  const handleDeleteBook = async (bookId) => {
    try {
      const token = sessionStorage.getItem("token");
      await axios.delete(
        `http://localhost:5289/api/Book/DeleteBook/${bookId}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      fetchBooks();
      setSuccessMessage("Book Deleted Successfully");
    } catch (error) {
      setError(error.message);
    }
  };

  const handleIsbnFilterChange = (event) => {
    setIsbnFilter(event.target.value);
  };

  const handleRackNumberFilterChange = (event) => {
    setRackNumberFilter(event.target.value);
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

  const generateReport = () => {
    const doc = new jsPDF();
    const allBooks = books;

    generatePage(doc, allBooks, "Books");

    doc.save("books_report.pdf");
  };

  const generatePage = (doc, bookList, title) => {
    doc.setFontSize(12);
    doc.text(`Report - ${title}`, 10, 10);
    doc.text("ID", 10, 20);
    doc.text("ISBN", 30, 20);
    doc.text("Rack Number", 60, 20);
    doc.text("Title", 100, 20);
    doc.text("Author", 130, 20);
    doc.text("Status", 160, 20);
    doc.text(`Total Number of Books: ${bookList.length}`, 10, 80);
    doc.text(
      `Total Number of Available Books: ${
        bookList.filter((book) => !book.isBorrowed).length
      }`,
      10,
      90
    );
    doc.text(
      `Total Number of Borrowed Books: ${
        bookList.filter((book) => book.isBorrowed).length
      }`,
      10,
      100
    );

    let y = 25;
    bookList.forEach((book) => {
      doc.text(book.id.toString(), 10, y);
      doc.text(book.isbn, 30, y);
      doc.text(book.rackNumber, 60, y);
      doc.text(book.title, 100, y);
      doc.text(book.author, 130, y);
      doc.text(book.isBorrowed ? "Borrowed" : "Available", 160, y);

      y += 10;
    });
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
        <div className="filter-container">
          <div className="isbn">
            <input
              type="text"
              placeholder="Filter by ISBN"
              value={isbnFilter}
              onChange={handleIsbnFilterChange}
              onKeyPress={handleFilterByIsbn}
            />
          </div>
          <div className="racknum">
            <input
              type="text"
              placeholder="Filter by Rack Number"
              value={rackNumberFilter}
              onChange={handleRackNumberFilterChange}
              onKeyPress={handleFilterByRackNumber}
            />
          </div>
          <Link to="/Books/AddBook">
            <button className="add-book-btn">Add Book</button>
          </Link>
          <button className="add-book-btn" onClick={generateReport}>
            Generate Book Report
          </button>
        </div>

        {isLoading ? (
          <p>Loading...</p>
        ) : error ? (
          <div className="error-message-container">
            <p className="error-message">{error}</p>
          </div>
        ) : books.length === 0 ? (
          <div className="no-books-container">
            <p>No Books Available</p>
          </div>
        ) : (
          <div>
            {successMessage && (
              <div className="success-message-container">
                <p className="success-message">{successMessage}</p>
              </div>
            )}

            <table className="book-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>ISBN</th>
                  <th>Rack Number</th>
                  <th>Author</th>
                  <th>Title</th>
                  <th>Is Borrowed</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {books.map((book) => (
                  <tr key={book.id}>
                    <td>{book.id}</td>
                    <td>{book.isbn}</td>
                    <td>{book.rackNumber}</td>
                    <td>{book.author}</td>
                    <td>{book.title}</td>
                    <td>{book.isBorrowed ? "Yes" : "No"}</td>
                    <td>
                      <button
                        className="book-delete-btn"
                        onClick={() => handleDeleteBook(book.id)}
                      >
                        Delete
                      </button>
                      <Link to={`UpdateBook/${book.id}`}>
                        <button className="add-book-btn">Update</button>
                      </Link>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

export default Books;
