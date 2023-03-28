import React from "react";
import Connect from "./Connect";

export default function Header() {
  return (
    <React.Fragment>
      <header className="header">
        <div className="logo-container">
          <img
            src={require("../images/logo.png").default}
            alt="Brand Logo"
            height="50px"
          />
        </div>
        <Connect />
      </header>
    </React.Fragment>
  );
}
