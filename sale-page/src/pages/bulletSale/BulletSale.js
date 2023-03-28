import React, { useState, useContext, useEffect } from "react";
import SaleCard from "./SaleCard";
import Context from "./context/Context";
import { useWeb3React } from "@web3-react/core";
import { defaultValue } from "./context/defaults";

export default function BulletSale() {
  const appContext = useContext(Context);
  const [context, setContext] = useState(appContext);
  const { chainId, account } = useWeb3React();

  const setValue = (value) => {
    setContext({ ...context, value });
  };

  useEffect(() => {
    setContext({ ...context, value: { ...defaultValue } });
    // eslint-disable-n7ext-line
  }, [chainId, account]);

  const state = {
    ...context,
    setValue: setValue,
  };

  return (
    <Context.Provider value={state}>
      <React.Fragment>
        <section className="explore-area prev-project-area">
          <div className="container">
            <div className="card">
              <div className="card-body">
                <form className="login-box">
                  <SaleCard />
                </form>
              </div>
            </div>
          </div>
        </section>
      </React.Fragment>
    </Context.Provider>
  );
}
