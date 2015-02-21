var Component = React.createClass({
    render: function () {

        var component;

        if (this.props.item.type === 'LightSwitch') {
            component = (
                <LightSwitch item={this.props.item} />
            );
        }
        else {
            component = (
                <div className="component">Generic - {this.props.item.name}</div>
            );
        }

        return (
          <div>
            {component}
          </div>
        );
    }
});