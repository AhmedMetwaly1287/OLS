import React, { useState, useEffect } from "react";
import axios from "axios";
import "./assets/css/UserRequests.css";
import Logout from "./Logout";
import { Link } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

const UserRequests = () => {
  const [requests, setRequests] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [successMessage, setSuccessMessage] = useState("");

  useEffect(() => {
    fetchRequests();
  }, []);

  const fetchRequests = async () => {
    setIsLoading(true);
    try {
      const token = sessionStorage.getItem("token");
      const decodedToken = jwtDecode(token);
      const response = await axios.get(
        `http://localhost:5289/api/BorrowedBook/GetUserRequests/${decodedToken.UserID}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      setRequests(response.data);
      setError(null);
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCancelRequest = async (requestID) => {
    setIsLoading(true);
    try {
      const token = sessionStorage.getItem("token");
      await axios.delete(
        `http://localhost:5289/api/BorrowedBook/DeleteRequest/${requestID}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      // Remove the deleted request from the requests array
      setRequests(
        requests.filter((request) => request.requestID !== requestID)
      );
      setSuccessMessage("Request Cancelled Successfully");
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  const handleReturnBook = async (requestID) => {
    setIsLoading(true);
    try {
      const token = sessionStorage.getItem("token");
      await axios.put(
        `http://localhost:5289/api/BorrowedBook/ReturnBook/${requestID}`,
        {},
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      // Remove the deleted request from the requests array
      setRequests();
      setSuccessMessage("Book Returned Successfully");
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="requests-container">
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
      <h3 className="page-title">Borrow Requests</h3>
      <div className="content">
        {successMessage && (
          <div className="book-success-message-container">
            <p className="success-message">{successMessage}</p>
          </div>
        )}
        {isLoading ? (
          <p>Loading...</p>
        ) : error ? (
          <p className="error-message">{error}</p>
        ) : requests && requests.length > 0 ? (
          <table className="requests-table">
            <thead>
              <tr>
                <th>Request ID</th>
                <th>Book ID</th>
                <th>Book Title</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {requests
                .filter((request) => request.returnDate === null)
                .map((request) => (
                  <tr key={request.requestID}>
                    <td>{request.requestID}</td>
                    <td>{request.bookID}</td>
                    <td>{request.title}</td>
                    <td>
                      {request.isApproved === null &&
                      request.returnDate === null
                        ? "Pending"
                        : null}
                      {request.isApproved === true &&
                      request.returnDate === null
                        ? "Approved"
                        : null}
                      {request.isApproved === false &&
                      request.returnDate !== null
                        ? "Rejected"
                        : null}
                      {request.isApproved === true &&
                      request.returnDate !== null
                        ? "Returned"
                        : null}
                    </td>
                    <td>
                      {request.returnDate === null ? (
                        request.isApproved === null ? (
                          <button
                            className="cancel-button"
                            onClick={() =>
                              handleCancelRequest(request.requestID)
                            }
                          >
                            Cancel
                          </button>
                        ) : (
                          <button
                            className="return-button"
                            onClick={() => handleReturnBook(request.requestID)}
                          >
                            Return
                          </button>
                        )
                      ) : request.isApproved ? (
                        "-"
                      ) : (
                        "-"
                      )}
                    </td>
                  </tr>
                ))}
            </tbody>
          </table>
        ) : (
          <div className="error-message">No Requests</div>
        )}
      </div>
    </div>
  );
};

export default UserRequests;
