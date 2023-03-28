import arbitrumIcon from "../images/networks/ic_arbitrum_24.svg";
import cronosIcon from "../images/networks/ic_arbitrum_24.svg";

export const supportNetwork = {
  25: {
    name: "Cronos Network",
    chainId: "25",
    rpc: "https://evm.cronos.org",
    scanUrl: "https://cronoscan.com/",
    image: arbitrumIcon,
    symbol: "ETH",
  },
  default: {
    name: "Cronos Network",
    chainId: "25",
    rpc: "https://evm.cronos.org",
    scanUrl: "https://cronoscan.com/",
    image: cronosIcon,
    symbol: "ETH",
  },
};

export const RPC_URLS = {
  25: "https://evm.cronos.org",
};
