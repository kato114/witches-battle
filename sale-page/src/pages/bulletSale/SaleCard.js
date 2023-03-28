import React, { useContext, useState } from "react";
import Context from "./context/Context";
import { getWeb3 } from "../../hooks/connectors";
import { toast } from "react-toastify";
import { contract } from "../../hooks/constant";
import { useWeb3React } from "@web3-react/core";
import { getContract, mulDecimal } from "../../hooks/contractHelper";
import tokenAbi from "../../json/token.json";
import Button from "react-bootstrap-button-loader";

export default function SaleCard() {
  const context = useWeb3React();
  const { chainId, account, library } = context;
  const { value, setValue } = useContext(Context);
  const [lockloading, setLockLoading] = useState(false);

  const onChangeInput = (e) => {
    e.preventDefault();
    if (e.target.name == "tokenAmount") {
      setValue({
        ...value,
        tokenAmount: e.target.value,
        bulletAmount: e.target.value * 1000,
      });
    } else {
      setValue({
        ...value,
        tokenAmount: e.target.value / 1000,
        bulletAmount: e.target.value,
      });
    }
  };

  const handleBuyBullet = async (e) => {
    e.preventDefault();
    try {
      let web3 = getWeb3(chainId);
      setLockLoading(true);
      let tokenAddress = contract[chainId]
        ? contract[chainId].tokenAddress
        : contract["default"].tokenAddress;
      let tokenContract = getContract(tokenAbi, tokenAddress, library);
      let mainWallet = contract[chainId]
        ? contract[chainId].mainWallet
        : contract["default"].mainWallet;

      let tx = await tokenContract.transfer(
        mainWallet,
        mulDecimal(value.tokenAmount, 9),
        { from: account }
      );
      const resolveAfter3Sec = new Promise((resolve) =>
        setTimeout(resolve, 5000)
      );
      toast.promise(resolveAfter3Sec, {
        pending: "Waiting for confirmation 👌",
      });
      let interval = setInterval(async function () {
        var response = await web3.eth.getTransactionReceipt(tx.hash);
        if (response != null) {
          clearInterval(interval);
          if (response.status === true) {
            toast.success("success ! your last transaction is success 👍");
            setLockLoading(false);
          } else if (response.status === false) {
            toast.error("error ! Your last transaction is failed.");
            setLockLoading(false);
          } else {
            toast.error("error ! something went wrong.");
            setLockLoading(false);
          }
        }
      }, 5000);
    } catch (err) {
      toast.error(err.reason ? err.reason : err.message);
      setLockLoading(false);
    }
  };

  return (
    <div className={`tab-pane active mt-5`} role="tabpanel" id="step1">
      <h5 className="text-center pb-4">Enjoy game to earn more</h5>
      <h3 className="text-center mt-5">EXCHANGE</h3>
      <h6 className="text-center pb-3">BUY BULLET WITH CROSPELL TOKEN</h6>
      <div className="sale-form mt-5">
        <input
          className="form-control"
          onChange={(e) => onChangeInput(e)}
          value={value.tokenAmount}
          type="text"
          name="tokenAmount"
        />
        <i class="fas fa-chevron-right"></i>
        <input
          className="form-control"
          onChange={(e) => onChangeInput(e)}
          value={value.bulletAmount}
          type="text"
          name="bulletAmount"
        />
      </div>
      <div className="status-container">
        <div className="status-content">
          <h6 className="text-center">Your Balance</h6>
          <div className="status-bar">
            <div className="status-token"></div>
            <div className="status-bullet"></div>
          </div>
          <div className="status-detail">
            <h6>700 CroSpell</h6>
            <h6>300 Bullet</h6>
          </div>
        </div>
      </div>
      <ul className="list-inline text-center mt-5">
        <li>
          <Button
            type="button"
            className="default-btn"
            loading={lockloading}
            onClick={(e) => handleBuyBullet(e)}
          >
            Buy Bullet
          </Button>
        </li>
      </ul>
    </div>
  );
}
