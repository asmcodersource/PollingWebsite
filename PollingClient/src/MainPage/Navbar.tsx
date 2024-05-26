import ReactNavbar from 'react-bootstrap/Navbar';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import './Navbar.css';
import 'bootstrap/dist/css/bootstrap.min.css';

export interface NavbarLink {
    id: number,
    url: string,
    name: string,
    handler: React.MouseEventHandler<HTMLButtonElement>,
};
    
const Navbar = (props: any) => {
    return (
        <ReactNavbar collapseOnSelect sticky="top" bg="dark" data-bs-theme="dark" expand="lg" className="bg-body-tertiary" >
            <Container>
                    <ReactNavbar.Brand href="#home">{props.brand}</ReactNavbar.Brand>
                    <ReactNavbar.Toggle aria-controls="basic-navbar-nav" />
                    <ReactNavbar.Collapse id="basic-navbar-nav">
                    <Nav className="me-auto">
                        {props.links.map((link: NavbarLink) => 
                            <Nav.Link className="text-start " onClick={link.handler} key={link.id} href={link.url}>{link.name}</Nav.Link>
                        )}
                    </Nav>
                </ReactNavbar.Collapse>
            </Container>
        </ReactNavbar>
    )
};

export default Navbar;