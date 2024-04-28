import React, { useState, useEffect } from "react";
import axios from "axios";
import "./assets/css/UserArchive.css";
import Logout from "./Logout";
import { Link } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

const UserArchive = () => {
  const [archives, setArchives] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchArchives();
  }, []);

  const fetchArchives = async () => {
    setIsLoading(true);
    try {
      const token = sessionStorage.getItem("token");
      const decodedToken = jwtDecode(token);
      const response = await axios.get(
        `http://localhost:5289/api/Archive/GetUserArchive/${decodedToken.UserID}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      setArchives(response.data);
      setError(null);
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
      <h3 className="page-title">User Archive</h3>
      <div className="content">
        {isLoading ? (
          <p>Loading...</p>
        ) : error ? (
          <p className="error-message">{error}</p>
        ) : (
          <table className="requests-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Borrow Request ID</th>
                <th>Book ID</th>
                <th>Book Title</th>
                <th>Date of Returnal</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {archives
                .filter((archive) => archive.returnDate !== null)
                .map((archive) => (
                  <tr key={archive.archiveID}>
                    <td>{archive.archiveID}</td>
                    <td>{archive.borrowedBookRequestID}</td>
                    <td>{archive.bookID}</td>
                    <td>{archive.title}</td>
                    <td>{archive.returnDate}</td>
                    <td>
                      {archive.isApproved === false &&
                      archive.returnDate !== null
                        ? "Rejected"
                        : null}
                      {archive.isApproved === true &&
                      archive.returnDate !== null
                        ? "Returned"
                        : null}
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

export default UserArchive;
