import logo from './logo.svg';
import './App.css';

import React from "react";
import { Route, Routes } from "react-router-dom";

import Login from "./pages/login";
import Layout from "./layout";

import { createBrowserHistory } from "history";

function getToken(){
  const tokenString = sessionStorage.getItem('data');
  if(tokenString != 'undefined'){
    const user = JSON.parse(tokenString);
    if(user){
      if(user.token == 'undefined'){
        return false;
      }else{
        return user.token;
      }
    }else{
      return false;
    }
  }else{
    return false;
  }
}

function App() {
  const token = getToken();
  const history = createBrowserHistory();
  if (!token) {
    history.push("login");
    return <Login />;
  }

  return (
    <div className='App App-header'>
      <Routes history={history}>
        {!token && <Route path="/login" element={<Login />} />}
        <Route path="*" element={<Layout />} />
      </Routes>
    </div>
  );
}

export default App;
