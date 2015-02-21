var Main = React.createClass({
    getInitialState: function() {
        return {components: []};
    },

    componentWillMount: function() {
        componentStore.getComponents(function(components) {
            this.setState({components: components })
        }.bind(this));
    },

    render: function () {

        var markup = this.state.components.map(function(item) {
            return (
                <li>
                    <Component item={item} />
                </li>
            );
        });

        return (
            <ul className="medium-block-grid-3 small-block-grid-2">
               {markup}
            </ul>
        );
    }
});