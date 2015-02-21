var LightSwitch = React.createClass({
    getInitialState: function() {
        return {
            // TODO: this is an 'anti-pattern' according to the docs.  State should be passed in.
            // But why would't a component own its own state?  The examples have it just bubble up the state change to the
            // top, then push it down from there.  That seems stupid.  Maybe a flux-style action system would help.
            switchState: this.props.item.state
        }
    },
    clickHandler: function() {
        var state = this.state.switchState;

        if (state === 'ON') state = 'OFF';
        else state = 'ON';

        componentStore.sendCommand(this.props.item, state);

        this.setState({ switchState: state });
    },
    render: function () {

        var classes = React.addons.classSet({
            'component': true,
            'lightSwitch': true,
            'on': this.state.switchState === 'ON',
            'off': this.state.switchState === 'OFF'
        });

        return (
          <div className={classes} onClick={this.clickHandler}>
            <i className="fa fa-lightbulb-o fa-4x"></i>
            Light Switch - {this.props.item.name}
          </div>
        );
    }
});