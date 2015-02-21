var Router = ReactRouter;

var DefaultRoute = Router.DefaultRoute;
var Link = Router.Link;
var Route = Router.Route;
var RouteHandler = Router.RouteHandler;

var App = React.createClass({
    render: function () {
        return (
          <div>
            <header>
              <ul>
                <li><Link to="app">Dashboard</Link></li>
                <li><Link to="components">Manage Components</Link></li>
                { /*<li><Link to="inbox">Inbox</Link></li>
                    <li><Link to="calendar">Calendar</Link></li> */ }
              </ul>
            </header>

        <RouteHandler/>
      </div>
      );
    }
});

var routes = (
  <Route name="app" path="/" handler={App}>
    <Route name="components" handler={ComponentList} />
    <Route name="addComponent" path="components/add" handler={AddComponent} />      
    <Route name="editComponent" path="components/edit/:id" handler={AddComponent} />
    <DefaultRoute handler={Main}/>

  {/*
    //<Route name="inbox" handler={Inbox}/>
    //<Route name="calendar" handler={Calendar}/>
    //<DefaultRoute handler={Dashboard}/>
    */}
  </Route>
);

Router.run(routes, function (Handler) {
    React.render(<Handler/>, document.getElementById('main'));
});