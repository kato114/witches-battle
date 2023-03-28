import Header from "../../component/Header";
import styled from "styled-components";

const MainContent = styled.div`
  width: 100%;
  min-height: 100vh;
`;

const MainLayout = (props) => {
  return (
    <MainContent className="container">
      <Header />
      <div className="main">{props.children}</div>
    </MainContent>
  );
};
export default MainLayout;
