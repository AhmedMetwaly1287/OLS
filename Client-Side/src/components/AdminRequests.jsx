import React, { useState, useEffect } from "react";
import axios from "axios";
import "./assets/css/AdminRequests.css";
import Logout from "./Logout";
import { Link } from "react-router-dom";

const AdminRequests = () => {
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
      const response = await axios.get(
        `http://localhost:5289/api/BorrowedBook/GetAll`,

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

  const handleApproveRequest = async (requestID) => {
    setIsLoading(true);
    try {
      const token = sessionStorage.getItem("token");
      await axios.put(
        `http://localhost:5289/api/BorrowedBook/ApproveRequest/${requestID}`,
        {},
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      setSuccessMessage("Request Approved Successfully");
      fetchRequests();
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  const handleRejectRequest = async (requestID) => {
    setIsLoading(true);
    try {
      const token = sessionStorage.getItem("token");
      await axios.put(
        `http://localhost:5289/api/BorrowedBook/RejectRequest/${requestID}`,
        {},
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      // Remove the deleted request from the requests array
      fetchRequests();
      setSuccessMessage("Request rejected successfully");
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
        ) : (
          <table className="requests-table">
            <thead>
              <tr>
                <th>Request ID</th>
                <th>User ID</th>
                <th>User Email</th>
                <th>Book ID</th>
                <th>Book Title</th>
                <th>Date of Borrowing</th>
                <th>Date of Returning</th>
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
                    <td>{request.userID}</td>
                    <td>{request.email}</td>
                    <td>{request.bookID}</td>
                    <td>{request.title}</td>
                    <td>{request.borrowedDate}</td>
                    <td>{request.returnDate}</td>
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
                      {request.isApproved === null ? (
                        <div>
                          <button
                            className="cancel-button"
                            onClick={() =>
                              handleApproveRequest(request.requestID)
                            }
                          >
                            Approve
                          </button>
                          <button
                            className="reject-button"
                            onClick={() =>
                              handleRejectRequest(request.requestID)
                            }
                          >
                            Reject
                          </button>
                        </div>
                      ) : (
                        <div>-</div>
                      )}
                    </td>
                  </tr>
                ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
};

export default AdminRequests;
