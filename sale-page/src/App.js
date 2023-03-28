import React from "react";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";

import MainLayout from "./layouts/main";

import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import BulletSale from "./pages/bulletSale/BulletSale";

function App() {
  return (
    <div className="App">
      <Router>
        <ToastContainer pauseOnFocusLoss={false} />
        <MainLayout>
          <Switch>
            <Route exact path="/">
              <BulletSale />
            </Route>
          </Switch>
        </MainLayout>
      </Router>
    </div>
  );
}

export default App;
